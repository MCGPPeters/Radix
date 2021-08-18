using System.Runtime.CompilerServices;
using System.Threading.Channels;
using System.Timers;
using Radix.Result;
using Radix.Validated;

namespace Radix;

/// <summary>
///     The bounded context is responsible for managing the runtime.
///     - Once an aggregate is created, it is never destroyed
///     - An aggregate can only acquire an id of an other aggregate when it is explicitly send to it or it has created
///     it
///     - The runtime is responsible to restoring the state of the aggregate when it is not alive within
///     the context or any other remote instance of the context within a cluster
///     - Only one instance of an aggregate will be alive
///     - One can only send commands that are scoped to the bounded context.
///     - All commands that are scoped to an aggregate MUST be subtypes of the command type scoped at the bounded context
///     level
///     - There is only one command type scoped and the level of the bounded context.
/// </summary>
/// <typeparam name="TCommand"></typeparam>
/// <typeparam name="TEvent"></typeparam>
/// <typeparam name="TFormat">The serialization format</typeparam>
public class BoundedContext<TCommand, TEvent, TFormat> : IDisposable
    where TEvent : notnull
{
    private readonly BoundedContextSettings<TEvent, TFormat> _boundedContextSettings;
    private readonly Dictionary<Id, Agent<TCommand, TEvent>> _registry = new();
    private readonly System.Timers.Timer _timer;

    private bool _disposedValue; // To detect redundant calls

    public BoundedContext(BoundedContextSettings<TEvent, TFormat> boundedContextSettings)
    {
        _boundedContextSettings = boundedContextSettings;
        _timer = new System.Timers.Timer(boundedContextSettings.GarbageCollectionSettings.ScanInterval.TotalMilliseconds) { AutoReset = true };
        _timer.Elapsed += RunGarbageCollection;
        _timer.Enabled = true;
    }

    public void UpdateGarbageCollectionSettings(GarbageCollectionSettings garbageCollectionSettings)
    {
        _timer.Interval = garbageCollectionSettings.ScanInterval.TotalMilliseconds;
        _boundedContextSettings.GarbageCollectionSettings = garbageCollectionSettings;
    }

    // This code added to correctly implement the disposable pattern.
    public void Dispose() => Dispose(true);

    private void RunGarbageCollection(object sender, ElapsedEventArgs e)
    {
        foreach ((Id id, Agent<TCommand, TEvent> agent) in _registry)
        {
            TimeSpan idleTime = DateTimeOffset.Now.Subtract(agent.LastActivity);
            if (idleTime < _boundedContextSettings.GarbageCollectionSettings.IdleTimeout)
            {
                continue;
            }

            agent.Channel?.Writer.Complete();
            agent.TokenSource?.Cancel();
            _registry.Remove(id);
        }
    }

    /// <summary>
    /// Create an instance for new (non-existant) aggregate
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="decide"></param>
    /// <param name="update"></param>
    /// <returns></returns>
    public Aggregate<TCommand, TEvent> Create<TState>(Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update)
        where TState : new() => Create(new Id(Guid.NewGuid()), decide, update, new NoneExistentVersion());


    /// <summary>
    /// Create an instance of an existing aggregate
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <param name="id"></param>
    /// <param name="decide"></param>
    /// <param name="update"></param>
    /// <param name="expectedVersion">The version of the aggregate to which the state should be restored. One can restore the state to every version the aggregate was ever at</param>
    /// <returns></returns>
    private Aggregate<TCommand, TEvent> Create<TState>(Id id, Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update, Version expectedVersion)
        where TState : new()
    {

        if (_registry.TryGetValue(id, out Agent<TCommand, TEvent>? actor))
        {
            // the possible null on the actor can be ignored. This point will not be reached
            return new Aggregate<TCommand, TEvent>(id, actor.Accept!);
        }

        DateTimeOffset lastActivity = DateTimeOffset.Now;
        var state = new TState();
        EventStreamDescriptor eventStreamDescriptor = new(typeof(TState).FullName, id);
        Channel<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>)> channel = Channel.CreateUnbounded<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>)>(new UnboundedChannelOptions { SingleReader = true });
        var tokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = tokenSource.Token;
        var agent = Task.Run(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();

            await foreach ((TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>) input in channel.Reader.ReadAllAsync(cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                (TransientCommandDescriptor<TCommand> commandDescriptor, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>> taskCompletionSource) = input;

                lastActivity = DateTimeOffset.Now;

                ConfiguredCancelableAsyncEnumerable<EventDescriptor<TEvent>> eventsSince = _boundedContextSettings
                    .GetEventsSince(commandDescriptor.Recipient, expectedVersion, eventStreamDescriptor.StreamIdentifier)
                    .OrderBy(descriptor => descriptor.CurrentVersion)
                    .ConfigureAwait(false);


                await foreach (EventDescriptor<TEvent> eventDescriptor in eventsSince)
                {
                    state = update(state, eventDescriptor.Event);
                    expectedVersion = eventDescriptor.CurrentVersion;
                }

                Result<TEvent[], CommandDecisionError> result = await decide(state, commandDescriptor.Command).ConfigureAwait(false);

                switch (result)
                {
                    case Ok<TEvent[], CommandDecisionError>(var events):

                        TransientEventDescriptor<TFormat>[]
                            eventDescriptors = events.Select(
                                @event => new TransientEventDescriptor<TFormat>(
                                    new EventType(@event.GetType()),
                                    _boundedContextSettings.Serialize(@event),
                                    _boundedContextSettings.SerializeMetaData(new EventMetaData(commandDescriptor.MessageId, commandDescriptor.CorrelationId)),
                                    new MessageId(Guid.NewGuid()))).ToArray();
                        Result<ExistingVersion, AppendEventsError> appendResult = await
                            _boundedContextSettings
                                .AppendEvents(commandDescriptor.Recipient, expectedVersion, eventStreamDescriptor, eventDescriptors)
                                .ConfigureAwait(false);

                        switch (appendResult)
                        {
                            case Ok<ExistingVersion, AppendEventsError>(var version):
                                // the events have been saved to the stream successfully. Update the state
                                state = update(state, events);

                                expectedVersion = version;

                                taskCompletionSource.SetResult(Ok<CommandResult<TEvent>, Error[]>(new CommandResult<TEvent> { Id = commandDescriptor.Recipient, Events = events, ExpectedVersion = expectedVersion }));
                                break;
                            case Error<ExistingVersion, AppendEventsError>(var error):
                                switch (error)
                                {
                                    case OptimisticConcurrencyError _:

                                        // todo => see if is a concurrency error according to business rules (custom rules)

                                        // re issue the transientCommand to try again
                                        await channel.Writer.WriteAsync((commandDescriptor, taskCompletionSource), cancellationToken).ConfigureAwait(false);

                                        break;
                                    default:

                                        throw new NotSupportedException();
                                }

                                break;
                            default:
                                taskCompletionSource.SetResult(Error<CommandResult<TEvent>, Error[]>(new Error[] { "Unexpected state" }));
                                break;
                        }

                        break;
                    case Error<TEvent[], CommandDecisionError>(var error):
                        taskCompletionSource.SetResult(Error<CommandResult<TEvent>, Error[]>(new Error[] { error.Message }));

                        break;
                }
            }
        }, cancellationToken);

        Accept<TCommand, TEvent> accept = CreateAccept<TState>()(id, decide, update);
        _registry.Add(id, new Agent<TCommand, TEvent> { Channel = channel, TokenSource = tokenSource, LastActivity = lastActivity, Task = agent, Accept = accept });


        return new Aggregate<TCommand, TEvent>(id, accept);
    }


    private Func<Id, Decide<TState, TCommand, TEvent>, Update<TState, TEvent>, Accept<TCommand, TEvent>> CreateAccept<TState>()
        where TState : new() => (id, decide, update)
        => async validatedCommand =>
        {
            switch (validatedCommand)
            {
                case Valid<TCommand>(var validCommand):
                    TransientCommandDescriptor<TCommand> transientCommandDescriptor = new(id, validCommand);
                    if (_registry.TryGetValue(transientCommandDescriptor.Recipient, out Agent<TCommand, TEvent>? actor))
                    {
                        return await Send(transientCommandDescriptor, actor).ConfigureAwait(false);
                    }

                    // schedule a dormant actor and recreate the state
                    Aggregate<TCommand, TEvent>? aggregate = Get(transientCommandDescriptor.Recipient, decide, update);
                    return await aggregate.Accept(validatedCommand);

                case Invalid<TCommand>(var messages):
                    return Error<CommandResult<TEvent>, Error[]>(messages.Select(s => new Error(s)).ToArray());
                default: throw new InvalidOperationException();
            }
        };

    private static async Task<Result<CommandResult<TEvent>, Error[]>> Send(TransientCommandDescriptor<TCommand> transientCommandDescriptor, Agent<TCommand, TEvent> actor)
    {
        var taskCompletionSource = new TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>();
        if (actor.Channel is not null)
        {
            await actor.Channel.Writer.WriteAsync((transientCommandDescriptor, taskCompletionSource)).ConfigureAwait(false);
        }

        return await taskCompletionSource.Task;
    }

    private void Dispose(bool disposing)
    {
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            _timer.Dispose();
        }

        _disposedValue = true;
    }

    public Aggregate<TCommand, TEvent> Get<TState>(Id id, Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update) where TState : new() => Create(id, decide, update, new ExistingVersion(0));
}

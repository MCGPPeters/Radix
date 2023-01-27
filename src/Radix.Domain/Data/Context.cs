
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using Radix.Data;
using Radix.Domain.Control;
using static Radix.Control.Result.Extensions;

namespace Radix.Domain.Data;

public interface Context<TCommand, TEvent, TFormat>
    where TEvent : notnull
{
    AppendEvents<TFormat> AppendEvents { get; }
    GetEventsSince<TEvent> GetEventsSince { get; }
    Serialize<TEvent, TFormat> Serialize { get; }
    Serialize<EventMetaData, TFormat> SerializeMetaData { get; }
    
    public Aggregate<TCommand, TEvent> Get<TState, TCommandHandler>(Aggregate.Id id)
        where TState : new()
        where TCommandHandler : CommandHandler<TState, TCommand, TEvent> => 
            Prelude.Memoize<Aggregate.Id, Aggregate<TCommand, TEvent>>(GetInternal<TState, TCommandHandler>)(id);

    private Aggregate<TCommand, TEvent> GetInternal<TState, TCommandHandler>(Aggregate.Id id)
        where TState : new()
        where TCommandHandler : CommandHandler<TState, TCommand, TEvent>
            => Create<TState, TCommandHandler>(id, new ExistingVersion(0));
        

    public Aggregate<TCommand, TEvent> Create<TState, TCommandHandler>()
        where TState : new()
        where TCommandHandler : CommandHandler<TState, TCommand, TEvent> =>
            Create<TState, TCommandHandler>((Aggregate.Id)Guid.NewGuid(), new NoneExistentVersion());


    private Aggregate<TCommand, TEvent> Create<TState, TCommandHandler>(Aggregate.Id id, Version expectedVersion)
        where TState : new()
        where TCommandHandler : CommandHandler<TState, TCommand, TEvent>
    {
        EventStreamDescriptor eventStreamDescriptor = new(typeof(TState).FullName, id);
        Channel<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>)> channel = Channel.CreateUnbounded<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>)>(new UnboundedChannelOptions { SingleReader = true });
        var tokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = tokenSource.Token;
        var agent = Task.Run(async () =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            var state = new TState();

            await foreach ((TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>) input in channel.Reader.ReadAllAsync(cancellationToken))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }

                (TransientCommandDescriptor<TCommand> commandDescriptor, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>> taskCompletionSource) = input;

                ConfiguredCancelableAsyncEnumerable<EventDescriptor<TEvent>> eventsSince =
                    GetEventsSince(commandDescriptor.Recipient, expectedVersion, eventStreamDescriptor.StreamIdentifier)
                    .OrderBy(descriptor => descriptor.CurrentVersion)
                    .ConfigureAwait(false);


                await foreach (EventDescriptor<TEvent> eventDescriptor in eventsSince)
                {
                    state = TCommandHandler.Update(state, new[] { eventDescriptor.Event });
                    expectedVersion = eventDescriptor.CurrentVersion;
                }

                Result<TEvent[], CommandDecisionError> result = await TCommandHandler.Decide(state, commandDescriptor.Command).ConfigureAwait(false);

                switch (result)
                {
                    case Ok<TEvent[], CommandDecisionError>(var events):

                        TransientEventDescriptor<TFormat>[]
                            eventDescriptors = events.Select(
                                @event => new TransientEventDescriptor<TFormat>(
                                    new EventType(@event.GetType()),
                                    Serialize(@event),
                                    SerializeMetaData(new EventMetaData(commandDescriptor.MessageId, commandDescriptor.CorrelationId)),
                                    (MessageId)Guid.NewGuid())).ToArray();
                        Result<ExistingVersion, AppendEventsError> appendResult = await
                                AppendEvents(commandDescriptor.Recipient, expectedVersion, eventStreamDescriptor, eventDescriptors)
                                .ConfigureAwait(false);

                        switch (appendResult)
                        {
                            case Ok<ExistingVersion, AppendEventsError>(var version):
                                // the events have been saved to the stream successfully. Update the state
                                state = TCommandHandler.Update(state, events);
                                expectedVersion = version;
                                taskCompletionSource.SetResult(Ok<CommandResult<TEvent>, Error[]>(new CommandResult<TEvent> { Id = commandDescriptor.Recipient, Events = events, ExpectedVersion = expectedVersion }));
                                break;
                            case Error<ExistingVersion, AppendEventsError>(var error):
                                switch (error)
                                {
                                    case OptimisticConcurrencyError _:
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
              

        static Aggregate<TCommand, TEvent> New(Aggregate.Id id, Channel<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>)> channel)
                => async validatedCommand
                    =>
                    {
                        switch (validatedCommand)
                        {
                            case Valid<TCommand>(var validCommand):
                                TransientCommandDescriptor<TCommand> transientCommandDescriptor = new(id, validCommand);
                                var taskCompletionSource = new TaskCompletionSource<Result<CommandResult<TEvent>, Error[]>>();
                                await channel.Writer.WriteAsync((transientCommandDescriptor, taskCompletionSource)).ConfigureAwait(false);

                                return await taskCompletionSource.Task;

                            case Invalid<TCommand>(var reasons):
                                return Error<CommandResult<TEvent>, Error[]>(reasons.Select(s => new Error { Message = s.Descriptions.Aggregate((current, next) => current + Environment.NewLine + next) }).ToArray());
                            default: throw new InvalidOperationException();
                        }
                    };

        var aggregate = New(id, channel);
        return aggregate;
    }
}



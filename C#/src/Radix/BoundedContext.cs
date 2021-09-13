
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Channels;
using Radix.Result;
using Radix.Validated;
using System.Runtime.CompilerServices;

namespace Radix;

public interface BoundedContext<TCommand, TEvent, TFormat>
    where TEvent : notnull
{ 
    private static readonly MemoryCache s_instances = new(new MemoryCacheOptions { });

    AppendEvents<TFormat> AppendEvents { get; }
    GetEventsSince<TEvent> GetEventsSince { get; }
    FromEventDescriptor<TEvent, TFormat> FromEventDescriptor { get; }
    Serialize<TEvent, TFormat> Serialize { get; }
    Serialize<EventMetaData, TFormat> SerializeMetaData { get; }

    public Aggregate<TCommand, TEvent> Get<TState, TCommandHandler>(Id id)
        where TState : new()
        where TCommandHandler : CommandHandler<TState, TCommand, TEvent, TCommandHandler>
    {
        if (!s_instances.TryGetValue(id, out object value))
        {
            Aggregate<TCommand, TEvent> aggregate = Create<TState, TCommandHandler>(id, new ExistingVersion(0));
            using (ICacheEntry cacheEntry = s_instances.CreateEntry(id))
            {
                value = (cacheEntry.Value = aggregate);
            }
            return aggregate;
        }

        return (Aggregate<TCommand, TEvent>)value;
    }

    public Aggregate<TCommand, TEvent> Create<TState, TCommandHandler>()
        where TState : new()
        where TCommandHandler : CommandHandler<TState, TCommand, TEvent, TCommandHandler> =>
            Create<TState, TCommandHandler>(new Id(Guid.NewGuid()), new NoneExistentVersion());


    private Aggregate<TCommand, TEvent> Create<TState, TCommandHandler>(Id id, Version expectedVersion)
        where TState : new()
        where TCommandHandler : CommandHandler<TState, TCommand, TEvent, TCommandHandler>
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
                                    new MessageId(Guid.NewGuid()))).ToArray();
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

        AggregateInstance<TCommand, TEvent> aggregate = new(id, channel, agent, tokenSource);
        var _ = s_instances.Set(id, aggregate);
        return aggregate;
    }
}



using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Radix.Monoid;
using Radix.Result;
using static Radix.Result.Extensions;

namespace Radix
{
    /// <summary>
    ///     Accepts command on behalf of an aggregate instance and maintains its state
    /// </summary>
    /// <typeparam name="TState"></typeparam>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TEvent"></typeparam>
    /// <typeparam name="TFormat"></typeparam>
    internal class AggregateActor<TState, TCommand, TEvent, TFormat> : Actor<TCommand, TEvent>
        where TState : new()
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
        where TEvent : class, Event
    {

        private readonly ActionBlock<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<TEvent[], Error[]>>)> _actionBlock;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly BoundedContextSettings<TEvent, TFormat> _boundedContextSettings;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private TState _state;

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="boundedContextSettings"></param>
        /// <param name="decide"></param>
        /// <param name="update"></param>
        private AggregateActor(Address address, BoundedContextSettings<TEvent, TFormat> boundedContextSettings,
            Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update)
        {
            _boundedContextSettings = boundedContextSettings;
            LastActivity = DateTimeOffset.Now;
            _state = new TState();
            Version expectedVersion = new NoneExistentVersion();
            EventStreamDescriptor eventStreamDescriptor = new EventStreamDescriptor(typeof(TState).FullName, address);

            _actionBlock = new ActionBlock<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<TEvent[], Error[]>>)>(
                async input =>
                {
                    (TransientCommandDescriptor<TCommand> commandDescriptor, TaskCompletionSource<Result<TEvent[], Error[]>> taskCompletionSource) = input;

                    if (commandDescriptor is null)
                    {
                        return;
                    }

                    LastActivity = DateTimeOffset.Now;

                    ConfiguredCancelableAsyncEnumerable<EventDescriptor<TEvent>> eventsSince = _boundedContextSettings
                        .GetEventsSince(commandDescriptor.Recipient, expectedVersion, eventStreamDescriptor.StreamIdentifier)
                        .OrderBy(descriptor => descriptor.ExistingVersion)
                        .ConfigureAwait(false);


                    await foreach (EventDescriptor<TEvent> eventDescriptor in eventsSince)
                    {
                        _state = update(_state, eventDescriptor.Event);
                        expectedVersion = eventDescriptor.ExistingVersion;
                    }

                    Result<TEvent[], CommandDecisionError> result = await decide(_state, commandDescriptor.Command).ConfigureAwait(false);

                    switch (result)
                    {
                        case Ok<TEvent[], CommandDecisionError>(var events):

                            TransientEventDescriptor<TFormat>[]
                                eventDescriptors = events.Select(
                                    @event =>
                                    {
                                        @event.Address = address;
                                        return _boundedContextSettings.ToTransientEventDescriptor(
                                            new MessageId(Guid.NewGuid()),
                                            @event,
                                            _boundedContextSettings.Serialize,
                                            new EventMetaData(commandDescriptor.MessageId, commandDescriptor.CorrelationId),
                                            _boundedContextSettings.SerializeMetaData);
                                    }).ToArray();
                            ConfiguredTaskAwaitable<Result<ExistingVersion, AppendEventsError>> appendResult =
                                _boundedContextSettings
                                    .AppendEvents(commandDescriptor.Recipient, expectedVersion, eventStreamDescriptor, eventDescriptors)
                                    .ConfigureAwait(false);

                            switch (await appendResult)
                            {
                                case Ok<ExistingVersion, AppendEventsError>(var version):
                                    // the events have been saved to the stream successfully. Update the state
                                    _state = update(_state, events);

                                    foreach (TEvent @event in events)
                                    {
                                        @event.Address = address;
                                    }

                                    expectedVersion = version;

                                    taskCompletionSource.SetResult(Ok<TEvent[], Error[]>(events));
                                    break;
                                case Error<ExistingVersion, AppendEventsError>(var error):
                                    switch (error)
                                    {
                                        case OptimisticConcurrencyError _:
                                            // re issue the transientCommand to try again
                                            await _actionBlock.SendAsync((commandDescriptor, taskCompletionSource)).ConfigureAwait(false);

                                            break;
                                        default:

                                            throw new NotSupportedException();
                                    }

                                    break;
                                default:
                                    taskCompletionSource.SetResult(Error<TEvent[], Error[]>(new Error[] {"Unexpected state"}));
                                    break;
                            }

                            break;
                        case Error<TEvent[], CommandDecisionError>(var error):
                            taskCompletionSource.SetResult(Error<TEvent[], Error[]>(new Error[] {error.Message}));

                            break;
                    }
                }
            );

        }

        public async Task<Result<TEvent[], Error[]>> Post(TransientCommandDescriptor<TCommand> transientCommandDescriptor)
        {
            TaskCompletionSource<Result<TEvent[], Error[]>> taskCompletionSource = new TaskCompletionSource<Result<TEvent[], Error[]>>();
            await _actionBlock.SendAsync((transientCommandDescriptor, taskCompletionSource)).ConfigureAwait(false);
            return await taskCompletionSource.Task.ConfigureAwait(false);
        }


        public DateTimeOffset LastActivity { get; set; }

        public void Deactivate() => _actionBlock.Complete();

        public static AggregateActor<TState, TCommand, TEvent, TFormat> Create(Address address, BoundedContextSettings<TEvent, TFormat> boundedContextSettings,
            Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update) =>
            new AggregateActor<TState, TCommand, TEvent, TFormat>(address, boundedContextSettings, decide, update);
    }


}

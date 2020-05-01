using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Radix.Monoid;
using Radix.Option;
using Radix.Result;
using static Radix.Result.Extensions;
using static Radix.Option.Extensions;

namespace Radix
{

    internal class AggregateAgent<TState, TCommand, TEvent> : Agent<TCommand, TEvent>
        where TState : new()
        where TEvent : Event
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {

        private readonly ActionBlock<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<TEvent[], Error[]>>)> _actionBlock;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;
        private readonly Decide<TState, TCommand, TEvent> _decide;
        private readonly Update<TState, TEvent> _update;

        private Version _expectedVersion;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private TState _state;

        private EventStreamDescriptor<TState> _eventStreamDescriptor;

        /// <summary>
        /// </summary>
        /// <param name="address"></param>
        /// <param name="boundedContextSettings"></param>
        /// <param name="decide"></param>
        /// <param name="update"></param>
        private AggregateAgent(Address address, BoundedContextSettings<TCommand, TEvent> boundedContextSettings,
            Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update)
        {
            _boundedContextSettings = boundedContextSettings;
            _decide = decide;
            _update = update;
            LastActivity = DateTimeOffset.Now;
            _state = new TState();
            _expectedVersion = new NoneExistentVersion();
            _eventStreamDescriptor = new NoneExistentEventStreamDescriptor<TState>(address);

            _actionBlock = new ActionBlock<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<TEvent[], Error[]>>)>(
                async input =>
                {
                    (TransientCommandDescriptor<TCommand> commandDescriptor, TaskCompletionSource<Result<TEvent[], Error[]>> taskCompletionSource) = input;

                    if (commandDescriptor is null)
                    {
                        return;
                    }

                    LastActivity = DateTimeOffset.Now;

                    ConfiguredCancelableAsyncEnumerable<EventDescriptor<TEvent>> eventsSince = _boundedContextSettings.GetEventsSince(commandDescriptor.Recipient, _expectedVersion, _eventStreamDescriptor.StreamIdentifier)
                        .OrderBy(descriptor => descriptor.ExistentVersion)
                        .ConfigureAwait(false);

                    await foreach (EventDescriptor<TEvent> eventDescriptor in eventsSince)
                    {

                        Option<Conflict<TCommand, TEvent>> optionalConflict = _boundedContextSettings.CheckForConflict(commandDescriptor.Command, eventDescriptor);

                        switch (optionalConflict)
                        {
                            case None<Conflict<TCommand, TEvent>> _:
                                _expectedVersion = eventDescriptor.ExistentVersion;
                                break;
                            case Some<Conflict<TCommand, TEvent>>(var conflict):
                                taskCompletionSource.SetResult(Error<TEvent[], Error[]>(new Error[] {conflict.Reason}));
                                return;
                        }
                    }

                    Result<TEvent[], CommandDecisionError> result = await _decide(_state, commandDescriptor).ConfigureAwait(false);

                    switch (result)
                    {
                        case Ok<TEvent[], CommandDecisionError>(var events):
                            TransientEventDescriptor<TEvent>[]
                                eventDescriptors = events.Select(@event => new TransientEventDescriptor<TEvent>(commandDescriptor, @event)).ToArray();
                            ConfiguredTaskAwaitable<Result<ExistentVersion, AppendEventsError>> appendResult =
                                _boundedContextSettings.AppendEvents(commandDescriptor.Recipient, _expectedVersion, _eventStreamDescriptor.StreamIdentifier, eventDescriptors).ConfigureAwait(false);

                            switch (await appendResult)
                            {
                                case Ok<ExistentVersion, AppendEventsError>(_):
                                    // the events have been saved to the stream successfully. Update the state
                                    _state = events.Aggregate(_state, _update.Invoke);

                                    taskCompletionSource.SetResult(Ok<TEvent[], Error[]>(events));
                                    break;
                                case Error<ExistentVersion, AppendEventsError>(var error):
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
                        case Error<TEvent[], CommandDecisionError> (var error):
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

        public static async Task<AggregateAgent<TState, TCommand, TEvent>> Create(Address address, BoundedContextSettings<TCommand, TEvent> boundedContextSettings,
            Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update)
        {
            return new AggregateAgent<TState, TCommand, TEvent>(address, boundedContextSettings, decide, update);
        }
    }

    public class ExistentEventStreamDescriptor<TState> : EventStreamDescriptor<TState>
    {
        public ExistentEventStreamDescriptor(Address address)
        {
            Address = address;
        }

        public ExistentVersion ExpectedExistentVersion { get; set; } = new ExistentVersion(0L);
        public Address Address { get;  }
    }

    public class NoneExistentEventStreamDescriptor<TState> : EventStreamDescriptor<TState>
    {
        public Address Address { get; }

        public NoneExistentEventStreamDescriptor(Address address)
        {
            Address = address;
        }
    }

    public interface EventStreamDescriptor<TState>
    {
        Address Address { get; }

        public string StreamIdentifier
        {
            get
            {
                return $"{typeof(TState)}-{Address}";
            }
        }
    }

}

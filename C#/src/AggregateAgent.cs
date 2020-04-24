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

        private Version _expectedVersion;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private TState _state;
        private Update<TState, TEvent> _update;

        /// <summary>
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="expectedVersion">The agent maintains the expected expectedVersion for sending commands</param>
        /// <param name="boundedContextSettings"></param>
        /// <param name="decide"></param>
        /// <param name="update"></param>
        private AggregateAgent(TState initialState, Version expectedVersion, BoundedContextSettings<TCommand, TEvent> boundedContextSettings, Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update)
        {
            _boundedContextSettings = boundedContextSettings;
            _decide = decide;
            _update = update;
            LastActivity = DateTimeOffset.Now;
            _state = initialState;
            _expectedVersion = expectedVersion;

            _actionBlock = new ActionBlock<(TransientCommandDescriptor<TCommand>, TaskCompletionSource<Result<TEvent[], Error[]>>)>(
                async input =>
                {
                    (TransientCommandDescriptor<TCommand> commandDescriptor, TaskCompletionSource<Result<TEvent[], Error[]>> taskCompletionSource) = input;

                    if (commandDescriptor is null)
                    {
                        return;
                    }

                    LastActivity = DateTimeOffset.Now;

                    ConfiguredCancelableAsyncEnumerable<EventDescriptor<TEvent>> eventsSince = _boundedContextSettings.EventStore
                        .GetEventsSince(commandDescriptor.Recipient, _expectedVersion)
                        .OrderBy(descriptor => descriptor.Version)
                        .ConfigureAwait(false);

                    await foreach (EventDescriptor<TEvent> eventDescriptor in eventsSince)
                    {

                        Option<Conflict<TCommand, TEvent>> optionalConflict = _boundedContextSettings.CheckForConflict(commandDescriptor.Command, eventDescriptor);

                        switch (optionalConflict)
                        {
                            case None<Conflict<TCommand, TEvent>> _:
                                _expectedVersion = eventDescriptor.Version;
                                break;
                            case Some<Conflict<TCommand, TEvent>>(var conflict):
                                taskCompletionSource.SetResult(Error<TEvent[], Error[]>(new Error[] {conflict.Reason}));
                                return;
                        }
                    }

                    Result<TEvent[], CommandDecisionError> result = await _decide(initialState, commandDescriptor).ConfigureAwait(false);

                    switch (result)
                    {
                        case Ok<TEvent[], CommandDecisionError>(var events):
                            TransientEventDescriptor<TEvent>[]
                                eventDescriptors = events.Select(@event => new TransientEventDescriptor<TEvent>(commandDescriptor, @event)).ToArray();
                            ConfiguredTaskAwaitable<Result<Version, AppendEventsError>> appendResult =
                                _boundedContextSettings.EventStore.AppendEvents(commandDescriptor.Recipient, _expectedVersion, eventDescriptors).ConfigureAwait(false);

                            switch (await appendResult)
                            {
                                case Ok<Version, AppendEventsError>(_):
                                    // the events have been saved to the stream successfully. Update the state
                                    _state = events.Aggregate(_state, _update.Invoke);

                                    taskCompletionSource.SetResult(Ok<TEvent[], Error[]>(events));
                                    break;
                                case Error<Version, AppendEventsError>(var error):
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

        public static async Task<AggregateAgent<TState, TCommand, TEvent>> Create(BoundedContextSettings<TCommand, TEvent> boundedContextSettings, Decide<TState, TCommand, TEvent> decide, Update<TState, TEvent> update,
            IAsyncEnumerable<EventDescriptor<TEvent>> history)
        {
            (TState state, Version currentVersion) x = await Initial<TState, TEvent>.State(history, update).ConfigureAwait(false);
            return new AggregateAgent<TState, TCommand, TEvent>(
                x.state,
                x.currentVersion,
                boundedContextSettings,
                decide,
                update);
        }
    }

}

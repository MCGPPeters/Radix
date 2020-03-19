using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Radix.Monoid;
using Radix.Option;
using Radix.Result;
using Radix.TaskResult;
using static Radix.Result.Extensions;

namespace Radix
{
    internal class AggregateAgent<TState, TCommand, TEvent> : Agent<TCommand, TEvent>
        where TState : Aggregate<TState, TEvent, TCommand>, IEquatable<TState>, new() where TEvent : Event
    {

        private readonly ActionBlock<(CommandDescriptor<TCommand>, TaskCompletionSource<Result<TEvent[], string[]>>)> _actionBlock;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private TState _state;

        /// <summary>
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="boundedContextSettings"></param>
        private AggregateAgent(TState initialState, BoundedContextSettings<TCommand, TEvent> boundedContextSettings)
        {
            _boundedContextSettings = boundedContextSettings;
            LastActivity = DateTimeOffset.Now;
            _state = initialState;

            _actionBlock = new ActionBlock<(CommandDescriptor<TCommand>, TaskCompletionSource<Result<TEvent[], string[]>>)>(
                async input =>
                {
                    var (commandDescriptor, taskCompletionSource) = input;

                    if (commandDescriptor is null) return;

                    LastActivity = DateTimeOffset.Now;
                    var expectedVersion = commandDescriptor.ExpectedVersion;

                    var eventsSince = _boundedContextSettings.EventStore.GetEventsSince(commandDescriptor.Address, expectedVersion).OrderBy(descriptor => descriptor.Version).ConfigureAwait(false);
                    await foreach (var eventDescriptor in eventsSince)
                    {
                        var optionalConflict = _boundedContextSettings.FindConflict(commandDescriptor.Command, eventDescriptor);
                        switch (optionalConflict)
                        {
                            case None<Conflict<TCommand, TEvent>> _:
                                expectedVersion = eventDescriptor.Version;
                                break;
                            case Some<Conflict<TCommand, TEvent>>(var conflict):
                                taskCompletionSource.SetResult(Error<TEvent[], string[]>(new []{conflict.Reason}));
                                return;
                        }
                    }

                    var result = await _state.Decide(commandDescriptor).ConfigureAwait(false);
                    switch (result)
                    {
                        case Ok<TEvent[], CommandDecisionError>(var events):
                            var appendResult = _boundedContextSettings.EventStore.AppendEvents(commandDescriptor.Address, expectedVersion, events).ConfigureAwait(false);
                            switch (await appendResult)
                            {
                                case Ok<Version, AppendEventsError>(_):
                                    // the events have been saved to the stream successfully. Update the state
                                    _state = events.Aggregate(_state, (s, @event) => s.Apply(@event));
                                    taskCompletionSource.SetResult(Ok<TEvent[], string[]>(events));
                                    break;
                                case Error<Version, AppendEventsError>(var error):
                                    switch (error)
                                    {
                                        case OptimisticConcurrencyError _:
                                            // re issue the command to try again
                                            await _actionBlock.SendAsync((commandDescriptor, taskCompletionSource)).ConfigureAwait(true);;
                                            break;
                                        default: throw new NotSupportedException();
                                    }

                                    break;
                                default: throw new NotSupportedException();
                            }

                            break;
                        case Error<TEvent[], CommandDecisionError> (var error):
                            taskCompletionSource.SetResult(Error<TEvent[], string[]>(error.Messages));
                            break;
                    }
                }
            );

        }

        public async Task<Result<TEvent[], string[]>> Post(CommandDescriptor<TCommand> commandDescriptor)
        {
            var taskCompletionSource = new TaskCompletionSource<Result<TEvent[], string[]>>();
            await _actionBlock.SendAsync((commandDescriptor, taskCompletionSource)).ConfigureAwait(false);
            return await taskCompletionSource.Task.ConfigureAwait(false);
        }


        public DateTimeOffset LastActivity { get; set; }

        public void Deactivate()
        {
            _actionBlock.Complete();
        }

        public static async Task<AggregateAgent<TState, TCommand, TEvent>> Create(BoundedContextSettings<TCommand, TEvent> boundedContextSettings,
            IAsyncEnumerable<EventDescriptor<TEvent>> history)
        {
            return new AggregateAgent<TState, TCommand, TEvent>(await Initial<TState, TEvent>.State(history).ConfigureAwait(false), boundedContextSettings);
        }
    }
}
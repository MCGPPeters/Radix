using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Radix.Monoid;
using Radix.Result;

namespace Radix
{
    internal class StatefulAgent<TState, TCommand, TEvent> : Agent<TCommand>
        where TState : Aggregate<TState, TEvent, TCommand>, new()
    {
        private readonly ActionBlock<CommandDescriptor<TCommand>> _actionBlock;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private TState _state = new TState();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="boundedContextSettings"></param>
        /// <param name="history">The history of events to replay when restoring the state</param>
        /// <param name="scheduler"></param>
        public StatefulAgent(BoundedContextSettings<TCommand, TEvent> boundedContextSettings, IEnumerable<EventDescriptor<TEvent>> history, TaskScheduler scheduler)
        {
            _boundedContextSettings = boundedContextSettings;

            // restore the state (if any)
            _state = history.Aggregate(_state, (state, eventDescriptor) => state.
                Apply(eventDescriptor.Event));


            _actionBlock = new ActionBlock<CommandDescriptor<TCommand>>(
                async commandDescriptor =>
                {
                    LastActivity = DateTimeOffset.Now;
                    var expectedVersion = commandDescriptor.ExpectedVersion;
                    var eventsSinceExpected = await _boundedContextSettings.GetEventsSince(commandDescriptor.Address, expectedVersion);
                    if (eventsSinceExpected.Any())
                    {
                        var conflicts = _boundedContextSettings.FindConflicts(commandDescriptor.Command, eventsSinceExpected);
                        if (conflicts.Any())
                        {
                            // a true concurrent exception according to business logic
                            await _boundedContextSettings.OnConflictingCommandRejected(conflicts, new TaskCompletionSource<IEnumerable<Conflict<TCommand, TEvent>>>());
                            return;
                        }

                        // no conflicts, set the expected version
                        expectedVersion = eventsSinceExpected.Select(eventDescriptor => eventDescriptor.Version).Max();
                    }

                    var transientEvents = _state.Decide(commandDescriptor.Command);
                    // try to save the events
                    var saveResult = await _boundedContextSettings.SaveEvents(commandDescriptor.Address, expectedVersion, transientEvents);

                    switch (saveResult)
                    {
                        case Ok<Version, SaveEventsError> _:
                            // the events have been saved to the stream successfully. Update the state
                            _state = transientEvents.Aggregate(_state, (s, @event) => s.
                                Apply(@event));
                            break;
                        case Error<Version, SaveEventsError>(var error):
                            switch (error)
                            {
                                case OptimisticConcurrencyError _:
                                    // re issue the command to try again
                                    Post(commandDescriptor);
                                    break;
                                default: throw new NotSupportedException();
                            }

                            break;
                        default: throw new NotSupportedException();
                    }
                },
                new ExecutionDataflowBlockOptions
                {
                    TaskScheduler = scheduler
                });
        }

        public void Post(CommandDescriptor<TCommand> commandDescriptor)
        {
            _actionBlock.Post(commandDescriptor);
        }

        public DateTimeOffset LastActivity { get; set; }

        public void Deactivate()
        {
            _actionBlock.Complete();
        }
    }

}
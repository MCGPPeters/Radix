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
        public static async Task<StatefulAgent<TState, TCommand, TEvent>> Create(BoundedContextSettings<TCommand, TEvent> boundedContextSettings, IAsyncEnumerable<EventDescriptor<TEvent>> history, TaskScheduler scheduler)
        {
            // restore the state (if any)
            var initialState = await history.AggregateAsync(new TState(), (state, eventDescriptor)
                => state.Apply(eventDescriptor.Event));
            
            return new StatefulAgent<TState, TCommand, TEvent>(initialState, boundedContextSettings, history, scheduler);
        }

        private readonly ActionBlock<CommandDescriptor<TCommand>> _actionBlock;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private TState _state = new TState();

        /// <summary>
        /// </summary>
        /// <param name="boundedContextSettings"></param>
        /// <param name="history">The history of events to replay when restoring the state</param>
        /// <param name="scheduler"></param>
        private StatefulAgent(TState initialState, BoundedContextSettings<TCommand, TEvent> boundedContextSettings, IAsyncEnumerable<EventDescriptor<TEvent>> history, TaskScheduler scheduler)
        {
            _boundedContextSettings = boundedContextSettings;
            LastActivity = DateTimeOffset.Now;
            _state = initialState;

            _actionBlock = new ActionBlock<CommandDescriptor<TCommand>>(
                async commandDescriptor =>
                {
                    LastActivity = DateTimeOffset.Now;
                    var expectedVersion = commandDescriptor.ExpectedVersion;
                    var eventsSinceExpected = _boundedContextSettings.GetEventsSince(commandDescriptor.Address, expectedVersion);
                    
                    if (await eventsSinceExpected.AnyAsync())
                    {
                        var conflicts = _boundedContextSettings.FindConflicts(commandDescriptor.Command, eventsSinceExpected);
                        if (await conflicts.AnyAsync())
                        {
                            // a true concurrent exception according to business logic
                            await _boundedContextSettings.OnConflictingCommandRejected(conflicts);
                            return;
                        }

                        // no conflicts, set the expected version
                        var versions =  eventsSinceExpected.Select(eventDescriptor => eventDescriptor.Version);

                        expectedVersion = await versions.MaxAsync();
                    }

                    var transientEvents = _state.Decide(commandDescriptor);
                    // try to save the events
                    var saveResult = await _boundedContextSettings.AppendEvents(commandDescriptor.Address, expectedVersion, transientEvents);

                    switch (saveResult)
                    {
                        case Ok<Version, SaveEventsError> _:
                            // the events have been saved to the stream successfully. Update the state
                            _state = transientEvents.Aggregate(_state, (s, @event) => s.Apply(@event));
                            break;
                        case Error<Version, SaveEventsError>(var error):
                            switch (error)
                            {
                                case OptimisticConcurrencyError _:
                                    // re issue the command to try again
                                    await Post(commandDescriptor);
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

        public async Task Post(CommandDescriptor<TCommand> commandDescriptor)
        {
            await _actionBlock.SendAsync(commandDescriptor);
        }

        public DateTimeOffset LastActivity { get; set; }

        public void Deactivate()
        {
            _actionBlock.Complete();
        }
    }

}

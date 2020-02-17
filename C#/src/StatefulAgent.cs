using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Radix.Monoid;
using Radix.Result;

namespace Radix
{
    internal class StatefulAgent<TState, TCommand, TEvent, TSettings> : Agent<TCommand>
        where TSettings : AggregateSettings<TCommand, TEvent>
        where TState : Aggregate<TState, TEvent, TCommand, TSettings>, new()
    {
        private readonly ActionBlock<CommandDescriptor<TCommand>> _actionBlock;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly TSettings _aggregateSettings;
        private TState _state = new TState();

        public StatefulAgent(BoundedContextSettings<TCommand, TEvent> boundedContextSettings, TaskScheduler scheduler,
            TSettings aggregateSettings)
        {
            _boundedContextSettings = boundedContextSettings;
            _aggregateSettings = aggregateSettings;

            _actionBlock = new ActionBlock<CommandDescriptor<TCommand>>(
                async commandDescriptor =>
                {
                    var expectedVersion = commandDescriptor.ExpectedVersion;
                    var eventsSinceExpected = await _boundedContextSettings.GetEventsSince(commandDescriptor.Address, expectedVersion);
                    if (eventsSinceExpected.Any())
                    {
                        var conflicts = _boundedContextSettings.FindConflicts(commandDescriptor.Command, eventsSinceExpected);
                        if (conflicts.Any())
                        {
                            // a true concurrent exception according to business logic
                            await _aggregateSettings.OnConflictingCommandRejected(conflicts);
                            return;
                        }

                        // no conflicts, set the expected version
                        expectedVersion = eventsSinceExpected.Select(eventDescriptor => eventDescriptor.Version).Max();
                    }

                    var transientEvents = _state.Decide(commandDescriptor.Command, _aggregateSettings);
                    // try to save the events
                    var saveResult = await _boundedContextSettings.SaveEvents(commandDescriptor.Address, expectedVersion, transientEvents);

                    switch (saveResult)
                    {
                        case Ok<Unit, SaveEventsError> _:
                            // the events have been saved to the stream successfully. Update the state
                            _state = transientEvents.Aggregate(_state, (s, @event) => s.
                                Apply(@event));
                            break;
                        case Error<Unit, SaveEventsError>(var error):
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
    }

}
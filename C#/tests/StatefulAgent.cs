using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Radix.Tests.Result;

namespace Radix.Tests
{
    internal class StatefulAgent<TState, TCommand, TEvent> : Agent<TCommand> where TState : Aggregate<TState, TEvent, TCommand>, new()
    {
        private readonly ActionBlock<CommandDescriptor<TCommand>> _actionBlock;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;
        private TState _state = new TState();

        public StatefulAgent(BoundedContextSettings<TCommand, TEvent> boundedContextSettings, TaskScheduler scheduler)
        {
            _boundedContextSettings = boundedContextSettings;

            _actionBlock = new ActionBlock<CommandDescriptor<TCommand>>(
                async commandDescriptor =>
                {
                    var expectedVersion = commandDescriptor.ExpectedVersion;
                    var eventsSinceExpected = await _boundedContextSettings.GetEventsSince(commandDescriptor.Address, expectedVersion);
                    if (eventsSinceExpected.Any())
                    {
                        var conflicts = _boundedContextSettings.FindConflicts(commandDescriptor.Command, eventsSinceExpected);
                        if (conflicts.Any())
                            // a true concurrent exception according to business logic
                            // todo : notify the issuer of the command
                            return;
                        // no conflicts, set the expected version
                        expectedVersion = eventsSinceExpected.Select(eventDescriptor => eventDescriptor.Version).Max();
                    }

                    var transientEvents = _state.Decide(commandDescriptor.Command);
                    // try to save the events
                    var saveResult = await _boundedContextSettings.SaveEvents(commandDescriptor.Address, expectedVersion, transientEvents);

                    switch (saveResult)
                    {
                        case Ok<Unit, SaveEventsError> _:
                            // the events have been saved to the stream successfully. Update the state
                            _state = transientEvents.Aggregate(_state, (s, @event) => s.Apply(@event));
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
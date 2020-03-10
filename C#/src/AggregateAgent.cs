using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Radix.Monoid;
using Radix.Option;
using Radix.Result;

namespace Radix
{
    internal class AggregateAgent<TState, TCommand, TEvent> : Agent<TCommand>
        where TState : Aggregate<TState, TEvent, TCommand>, IEquatable<TState>, new() where TEvent : Event
    {
        public static async Task<AggregateAgent<TState, TCommand, TEvent>> Create(BoundedContextSettings<TCommand, TEvent> boundedContextSettings, IAsyncEnumerable<EventDescriptor<TEvent>> history, TaskScheduler scheduler) 
            => new AggregateAgent<TState, TCommand, TEvent>(await Initial<TState, TEvent>.State(history), boundedContextSettings, scheduler);

        private readonly ActionBlock<CommandDescriptor<TCommand>> _actionBlock;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private readonly BoundedContextSettings<TCommand, TEvent> _boundedContextSettings;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable => prevent implicit closure
        private TState _state;

        /// <summary>
        /// </summary>
        /// <param name="initialState"></param>
        /// <param name="boundedContextSettings"></param>
        /// <param name="scheduler"></param>
        private AggregateAgent(TState initialState, BoundedContextSettings<TCommand, TEvent> boundedContextSettings, TaskScheduler scheduler)
        {
            _boundedContextSettings = boundedContextSettings;
            LastActivity = DateTimeOffset.Now;
            _state = initialState;

            _actionBlock = new ActionBlock<CommandDescriptor<TCommand>>(
                async commandDescriptor =>
                {
                    if (commandDescriptor is null) return;

                    LastActivity = DateTimeOffset.Now;
                    var expectedVersion = commandDescriptor.ExpectedVersion;

                    var eventsSince = _boundedContextSettings.GetEventsSince(commandDescriptor.Address, expectedVersion).OrderBy(descriptor => descriptor.Version);
                    await foreach(var eventDescriptor in eventsSince)
                    {
                        var optionalConflict = _boundedContextSettings.FindConflict(commandDescriptor.Command, eventDescriptor);
                        switch (optionalConflict)
                        {
                            case None<Conflict<TCommand, TEvent>> _:
                                expectedVersion = eventDescriptor.Version;
                                break;
                            case Some<Conflict<TCommand, TEvent>> (var conflict):
                                await _boundedContextSettings.OnConflictingCommandRejected(conflict);
                                return;
                        }
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

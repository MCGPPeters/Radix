using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Radix.Monoid;
using Radix.Option;
using Radix.Result;
using static Radix.Result.Extensions;

namespace Radix
{
    internal class AggregateAgent<TState, TCommand, TEvent> : Agent<TCommand, TEvent>
        where TState : Aggregate<TState, TEvent, TCommand>, IEquatable<TState>, new() where TEvent : Event where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {

        private readonly ActionBlock<(CommandDescriptor<TCommand>, TaskCompletionSource<Result<TEvent[], Error[]>>)> _actionBlock;

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

            _actionBlock = new ActionBlock<(CommandDescriptor<TCommand>, TaskCompletionSource<Result<TEvent[], Error[]>>)>(
                async input =>
                {
                    var (commandDescriptor, taskCompletionSource) = input;

                    if (commandDescriptor is null) return;

                    LastActivity = DateTimeOffset.Now;
                    var expectedVersion = commandDescriptor.ExpectedVersion;

                    Console.Out.WriteLine("getting histroy");
                    var eventsSince = _boundedContextSettings.EventStore.GetEventsSince(commandDescriptor.Address, expectedVersion).OrderBy(descriptor => descriptor.Version)
                        .ConfigureAwait(false);
                    Console.Out.WriteLine("getting histroy done");
                    await foreach (var eventDescriptor in eventsSince)
                    {
                        Console.Out.WriteLine("checking for conflicts");
                        var optionalConflict = _boundedContextSettings.CheckForConflict(commandDescriptor.Command, eventDescriptor);
                        Console.Out.WriteLine("done checking for conflicts");
                        switch (optionalConflict)
                        {
                            case None<Conflict<TCommand, TEvent>> _:
                                expectedVersion = eventDescriptor.Version;
                                break;
                            case Some<Conflict<TCommand, TEvent>>(var conflict):
                                taskCompletionSource.SetResult(Error<TEvent[], Error[]>(new Error[] {conflict.Reason}));
                                return;
                        }
                    }

                    Console.Out.WriteLine("decide");
                    var result = await _state.Decide(commandDescriptor).ConfigureAwait(false);
                    Console.Out.WriteLine("decide done");
                    switch (result)
                    {
                        case Ok<TEvent[], CommandDecisionError>(var events):
                            var appendResult = _boundedContextSettings.EventStore.AppendEvents(commandDescriptor.Address, expectedVersion, events).ConfigureAwait(false);
                            Console.Out.WriteLine("append done");
                            switch (await appendResult)
                            {
                                case Ok<Version, AppendEventsError>(_):
                                    // the events have been saved to the stream successfully. Update the state
                                    _state = events.Aggregate(_state, (s, @event) => s.Apply(@event));
                                    Console.Out.WriteLine("append succesfull");
                                    taskCompletionSource.SetResult(Ok<TEvent[], Error[]>(events));
                                    break;
                                case Error<Version, AppendEventsError>(var error):
                                    switch (error)
                                    {
                                        case OptimisticConcurrencyError _:
                                            // re issue the command to try again
                                            await _actionBlock.SendAsync((commandDescriptor, taskCompletionSource)).ConfigureAwait(false);
                                            Console.Out.WriteLine("conflict, retrying");
                                            break;
                                        default:
                                            Console.Out.WriteLine("append failed because of unknown reason");
                                            throw new NotSupportedException();
                                    }

                                    break;
                                default:
                                    taskCompletionSource.SetResult(Error<TEvent[], Error[]>(new Error[] {"Unexpected state"}));
                                    break;
                            }

                            break;
                        case Error<TEvent[], CommandDecisionError> (var error):
                            Console.Out.WriteLine($"append not successfull {error.Message}");
                            taskCompletionSource.SetResult(Error<TEvent[], Error[]>(new Error[] {error.Message}));

                            break;
                    }
                }
            );

        }

        public async Task<Result<TEvent[], Error[]>> Post(CommandDescriptor<TCommand> commandDescriptor)
        {
            var taskCompletionSource = new TaskCompletionSource<Result<TEvent[], Error[]>>();
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

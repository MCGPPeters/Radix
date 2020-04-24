using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix
{

    public delegate TState Create<TState, TEvent>(IAsyncEnumerable<TEvent> history, Update<TState, TEvent> update);
    public delegate Task<Result<TEvent[], CommandDecisionError>> Decide<in TState, TCommand, TEvent>(TState state, TransientCommandDescriptor<TCommand> commandDescriptor) where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>;

    public static class StateExtensions
    {
        public static async Task<TState> Create<TState, TEvent>(IAsyncEnumerable<TEvent> history, Update<TState, TEvent> update) where TState : new()
        {
            TState state = new TState();

            // restore the initialState (if any)
            if (history is object)
            {
                state = await history.AggregateAsync(state, update.Invoke);
            }

            return state;
        }
    }
}

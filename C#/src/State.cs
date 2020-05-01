using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix
{
    public static class State
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

        public static async Task<(TState, ExistentVersion currentVersion)> Create<TState, TEvent>(IAsyncEnumerable<EventDescriptor<TEvent>> history, Update<TState, TEvent> update)
            where TState : new()
        {
            TState state = new TState();
            ExistentVersion currentExistentVersion = 0L;

            await foreach (EventDescriptor<TEvent> eventDescriptor in history)
            {
                state = update(state, eventDescriptor.Event);
                currentExistentVersion = eventDescriptor.ExistentVersion;
            }

            return (state, currentExistentVersion);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix
{
    public static class Initial<TState, TEvent>
        where TState : State<TState, TEvent>, IEquatable<TState>, new()
        where TEvent : Event
    {
        public static async Task<(TState, Version currentVersion)> State(IAsyncEnumerable<EventDescriptor<TEvent>> history)
        {
            TState initialState = new TState();
            Version currentVersion = 0L;

            await foreach (EventDescriptor<TEvent> eventDescriptor in history)
            {
                initialState.Update(eventDescriptor.Event);
                currentVersion = eventDescriptor.Version;
            }

            return (initialState, currentVersion);
        }

        public static async Task<TState> State(IAsyncEnumerable<TEvent> history)
        {
            TState initialState = new TState();

            // restore the initialState (if any)
            if (history is object)
            {
                initialState = await history.AggregateAsync(
                    initialState,
                    (state, @event)
                        => state.Update(@event));
            }

            return initialState;
        }
    }
}

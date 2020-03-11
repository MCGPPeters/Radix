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
        public static async Task<TState> State(IAsyncEnumerable<EventDescriptor<TEvent>> history)
        {
            var initialState = new TState();

            // restore the initialState (if any)
            if (history is object)
                initialState = await history.AggregateAsync(
                    initialState,
                    (state, eventDescriptor)
                        => state.Apply(eventDescriptor.Event));

            return initialState;
        }

        public static async Task<TState> State(IAsyncEnumerable<TEvent> history)
        {
            var initialState = new TState();

            // restore the initialState (if any)
            if (history is object)
                initialState = await history.AggregateAsync(
                    initialState,
                    (state, @event)
                        => state.Apply(@event));

            return initialState;
        }
    }
}

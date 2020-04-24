using System.Collections.Generic;
using System.Threading.Tasks;

namespace Radix
{
    public static class Initial<TState, TEvent>
        where TState : new()
        where TEvent : Event
    {
        public static async Task<(TState, Version currentVersion)> State(IAsyncEnumerable<EventDescriptor<TEvent>> history, Update<TState, TEvent> update)
        {
            TState state = new TState();
            Version currentVersion = 0L;

            await foreach (EventDescriptor<TEvent> eventDescriptor in history)
            {
                state = update(state, eventDescriptor.Event);
                currentVersion = eventDescriptor.Version;
            }

            return (state, currentVersion);
        }
    }
}

namespace Radix.Tests
{
    public class BoundedContextSettings<TCommand, TEvent>
    {
        public BoundedContextSettings(SaveEvents<TEvent> saveEvents, GetEventsSince<TEvent> getEventsSince, ResolveRemoteAddress resolveRemoteAddress, Forward<TCommand> forward,
            FindConflicts<TCommand, TEvent> findConflicts)
        {
            SaveEvents = saveEvents;
            GetEventsSince = getEventsSince;
            ResolveRemoteAddress = resolveRemoteAddress;
            Forward = forward;
            FindConflicts = findConflicts;
        }

        public SaveEvents<TEvent> SaveEvents { get; }
        public GetEventsSince<TEvent> GetEventsSince { get; }
        public ResolveRemoteAddress ResolveRemoteAddress { get; }
        public Forward<TCommand> Forward { get; }
        public FindConflicts<TCommand, TEvent> FindConflicts { get; }
    }
}
namespace Radix
{
    public class BoundedContextSettings<TCommand, TEvent>
    {
        public BoundedContextSettings(AppendEvents<TEvent> appendEvents, GetEventsSince<TEvent> getEventsSince, ResolveRemoteAddress resolveRemoteAddress, Forward<TCommand> forward,
            FindConflicts<TCommand, TEvent> findConflicts, OnConflictingCommandRejected<TCommand, TEvent> onConflictingCommandRejected,
            GarbageCollectionSettings garbageCollectionSettings)
        {
            AppendEvents = appendEvents;
            GetEventsSince = getEventsSince;
            ResolveRemoteAddress = resolveRemoteAddress;
            Forward = forward;
            FindConflicts = findConflicts;
            OnConflictingCommandRejected = onConflictingCommandRejected;
            GarbageCollectionSettings = garbageCollectionSettings;
        }

        public AppendEvents<TEvent> AppendEvents { get; }
        public GetEventsSince<TEvent> GetEventsSince { get; }
        public ResolveRemoteAddress ResolveRemoteAddress { get; }
        public Forward<TCommand> Forward { get; }
        public FindConflicts<TCommand, TEvent> FindConflicts { get; }

        public OnConflictingCommandRejected<TCommand, TEvent> OnConflictingCommandRejected { get; }
        public GarbageCollectionSettings GarbageCollectionSettings { get; }
    }

}

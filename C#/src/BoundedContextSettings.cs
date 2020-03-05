namespace Radix
{
    public class BoundedContextSettings<TCommand, TEvent> where TEvent : Event
    {
        public BoundedContextSettings(AppendEvents<TEvent> appendEvents, GetEventsSince<TEvent> getEventsSince, ResolveRemoteAddress resolveRemoteAddress, Forward<TCommand> forward,
            FindConflict<TCommand, TEvent> findConflict, OnConflictingCommandRejected<TCommand, TEvent> onConflictingCommandRejected,
            GarbageCollectionSettings garbageCollectionSettings)
        {
            AppendEvents = appendEvents;
            GetEventsSince = getEventsSince;
            ResolveRemoteAddress = resolveRemoteAddress;
            Forward = forward;
            FindConflict = findConflict;
            OnConflictingCommandRejected = onConflictingCommandRejected;
            GarbageCollectionSettings = garbageCollectionSettings;
        }

        public AppendEvents<TEvent> AppendEvents { get; }
        public GetEventsSince<TEvent> GetEventsSince { get; }
        public ResolveRemoteAddress ResolveRemoteAddress { get; }
        public Forward<TCommand> Forward { get; }
        public FindConflict<TCommand, TEvent> FindConflict { get; }

        public OnConflictingCommandRejected<TCommand, TEvent> OnConflictingCommandRejected { get; }
        public GarbageCollectionSettings GarbageCollectionSettings { get; }
    }

}

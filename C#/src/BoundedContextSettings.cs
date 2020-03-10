namespace Radix
{
    public class BoundedContextSettings<TCommand, TEvent> where TEvent : Event
    {
        private readonly EventStore<TEvent> _eventStore;

        public BoundedContextSettings(EventStore<TEvent> eventStore,
            FindConflict<TCommand, TEvent> findConflict, OnConflictingCommandRejected<TCommand, TEvent> onConflictingCommandRejected,
            GarbageCollectionSettings garbageCollectionSettings)
        {
            _eventStore = eventStore;
            FindConflict = findConflict;
            OnConflictingCommandRejected = onConflictingCommandRejected;
            GarbageCollectionSettings = garbageCollectionSettings;
        }

        public FindConflict<TCommand, TEvent> FindConflict { get; }

        public OnConflictingCommandRejected<TCommand, TEvent> OnConflictingCommandRejected { get; }
        public GarbageCollectionSettings GarbageCollectionSettings { get; }

        public EventStore<TEvent> EventStore => _eventStore;
    }

}

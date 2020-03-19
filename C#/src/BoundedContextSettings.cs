namespace Radix
{
    public class BoundedContextSettings<TCommand, TEvent> 
        where TEvent : Event
    {

        public BoundedContextSettings(EventStore<TEvent> eventStore,
            FindConflict<TCommand, TEvent> findConflict, 
            GarbageCollectionSettings garbageCollectionSettings)
        {
            EventStore = eventStore;
            FindConflict = findConflict;
            GarbageCollectionSettings = garbageCollectionSettings;
        }

        public FindConflict<TCommand, TEvent> FindConflict { get; }
        public GarbageCollectionSettings GarbageCollectionSettings { get; }

        public EventStore<TEvent> EventStore { get; }
    }

}

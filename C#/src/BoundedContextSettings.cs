namespace Radix
{
    public class BoundedContextSettings<TCommand, TEvent> 
        where TEvent : Event
    {

        public BoundedContextSettings(EventStore<TEvent> eventStore,
            CheckForConflict<TCommand, TEvent> checkForConflict, 
            GarbageCollectionSettings garbageCollectionSettings)
        {
            EventStore = eventStore;
            CheckForConflict = checkForConflict;
            GarbageCollectionSettings = garbageCollectionSettings;
        }

        public CheckForConflict<TCommand, TEvent> CheckForConflict { get; }
        public GarbageCollectionSettings GarbageCollectionSettings { get; }

        public EventStore<TEvent> EventStore { get; }
    }

}

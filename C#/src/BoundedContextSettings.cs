namespace Radix
{
    public class BoundedContextSettings<TCommand, TEvent>
        where TEvent : Event
    {

        public BoundedContextSettings(AppendEvents<TEvent> appendEvents, GetEventsSince<TEvent> getEventsSince,
            CheckForConflict<TCommand, TEvent> checkForConflict,
            GarbageCollectionSettings garbageCollectionSettings)
        {
            AppendEvents = appendEvents;
            GetEventsSince = getEventsSince;
            CheckForConflict = checkForConflict;
            GarbageCollectionSettings = garbageCollectionSettings;
        }

        public AppendEvents<TEvent> AppendEvents { get; }
        public GetEventsSince<TEvent> GetEventsSince { get; }
        public CheckForConflict<TCommand, TEvent> CheckForConflict { get; }
        public GarbageCollectionSettings GarbageCollectionSettings { get; }
    }

}

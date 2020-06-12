namespace Radix
{

    public class BoundedContextSettings<TCommand, TEvent, TFormat>
    {

        public BoundedContextSettings(AppendEvents<TFormat> appendEvents, GetEventsSince<TEvent> getEventsSince,
            CheckForConflict<TCommand, TEvent> checkForConflict,
            GarbageCollectionSettings garbageCollectionSettings, FromEventDescriptor<TEvent, TFormat> fromEventDescriptor,
            ToTransientEventDescriptor<TEvent, TFormat> toTransientEventDescriptor, Serialize<TEvent, TFormat> serialize, Serialize<EventMetaData, TFormat> serializeMetaData)
        {
            AppendEvents = appendEvents;
            GetEventsSince = getEventsSince;
            CheckForConflict = checkForConflict;
            GarbageCollectionSettings = garbageCollectionSettings;
            FromEventDescriptor = fromEventDescriptor;
            ToTransientEventDescriptor = toTransientEventDescriptor;
            Serialize = serialize;
            SerializeMetaData = serializeMetaData;
        }

        public AppendEvents<TFormat> AppendEvents { get; }
        public GetEventsSince<TEvent> GetEventsSince { get; }
        public CheckForConflict<TCommand, TEvent> CheckForConflict { get; }
        public GarbageCollectionSettings GarbageCollectionSettings { get; }
        public FromEventDescriptor<TEvent, TFormat> FromEventDescriptor { get; }
        public ToTransientEventDescriptor<TEvent, TFormat> ToTransientEventDescriptor { get; }

        public Serialize<TEvent, TFormat> Serialize { get; }

        public Serialize<EventMetaData, TFormat> SerializeMetaData { get; }
    }

}

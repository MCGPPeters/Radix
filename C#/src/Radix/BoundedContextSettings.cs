namespace Radix
{

    public class BoundedContextSettings<TEvent, TFormat> where TEvent : notnull
    {

        public BoundedContextSettings(AppendEvents<TFormat> appendEvents, GetEventsSince<TEvent> getEventsSince,
            GarbageCollectionSettings garbageCollectionSettings, FromEventDescriptor<TEvent, TFormat> fromEventDescriptor,
            ToTransientEventDescriptor<TEvent, TFormat> toTransientEventDescriptor, Serialize<TEvent, TFormat> serialize, Serialize<EventMetaData, TFormat> serializeMetaData)
        {
            AppendEvents = appendEvents;
            GetEventsSince = getEventsSince;
            GarbageCollectionSettings = garbageCollectionSettings;
            FromEventDescriptor = fromEventDescriptor;
            ToTransientEventDescriptor = toTransientEventDescriptor;
            Serialize = serialize;
            SerializeMetaData = serializeMetaData;
        }

        public AppendEvents<TFormat> AppendEvents { get; }
        public GetEventsSince<TEvent> GetEventsSince { get; }
        public GarbageCollectionSettings GarbageCollectionSettings { get; }
        public FromEventDescriptor<TEvent, TFormat> FromEventDescriptor { get; }
        public ToTransientEventDescriptor<TEvent, TFormat> ToTransientEventDescriptor { get; }

        public Serialize<TEvent, TFormat> Serialize { get; }

        public Serialize<EventMetaData, TFormat> SerializeMetaData { get; }
    }

}

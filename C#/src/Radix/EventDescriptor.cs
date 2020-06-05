namespace Radix
{
    /// <summary>
    ///     Combining the metadata of a persisted event with the event itself
    /// </summary>
    public class EventDescriptor<TFormat>
    {

        public EventDescriptor(TFormat @event, ExistingVersion existingVersion, EventType eventType)
        {
            Event = @event;
            ExistingVersion = existingVersion;
            EventType = eventType;
        }

        public TFormat Event { get; }
        public EventType EventType { get; }

        public ExistingVersion ExistingVersion { get; }
    }

}

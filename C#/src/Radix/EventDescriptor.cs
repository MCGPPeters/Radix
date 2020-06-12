namespace Radix
{
    /// <summary>
    ///     Combining the metadata of a persisted event with the event itself
    /// </summary>
    public class EventDescriptor<TEvent>
    {
        private TEvent _event;
        private ExistingVersion _existentVersion;
        private EventType _eventType;

        public EventDescriptor(TEvent @event, ExistingVersion existingVersion, EventType eventType)
        {
            Event = @event;
            ExistingVersion = existingVersion;
            EventType = eventType;
        }

        public TEvent Event { get; }

        public ExistingVersion ExistingVersion { get; }
        public EventType EventType { get; }
    }
}

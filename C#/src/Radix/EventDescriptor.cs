namespace Radix
{
    /// <summary>
    ///     Combining the metadata of a persisted event with the event itself
    /// </summary>
    public class EventDescriptor<TEvent>
    {
        public EventDescriptor(TEvent @event, ExistingVersion currentVersion, EventType eventType)
        {
            Event = @event;
            CurrentVersion = currentVersion;
            EventType = eventType;
        }

        public TEvent Event { get; }

        public ExistingVersion CurrentVersion { get; }
        public EventType EventType { get; }
    }
}

using System.Collections.Generic;
using System.Text.Json;

namespace Radix
{
    /// <summary>
    ///     Combining the metadata of a persisted event with the event itself
    /// </summary>
    public class EventDescriptor<TFormat>
    {

        public EventDescriptor(Address aggregate, TFormat eventMetaData, TFormat @event, ExistingVersion existingVersion, EventType eventType)
        {
            Event = @event;
            Aggregate = aggregate;
            EventMetaData = eventMetaData;
            ExistingVersion = existingVersion;
            EventType = eventType;
        }

        public TFormat Event { get; }
        public EventType EventType { get; }
        public Address Aggregate { get; }
        public TFormat EventMetaData { get; }

        public ExistingVersion ExistingVersion { get; }
    }

}

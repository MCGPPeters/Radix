using System;

namespace Radix
{
    [Serializable]
    public class EventStreamDescriptor
    {
        public EventStreamDescriptor()
        {
            
        }
        public EventStreamDescriptor(EventType eventType, Address address)
        {
            EventType = eventType;
            Address = address;
        }

        public string StreamIdentifier => $"{EventType}-{Address}";

        private EventType? EventType { get; set; }
        private Address? Address { get; set; }
    }
}

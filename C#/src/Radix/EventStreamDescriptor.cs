using System;

namespace Radix
{
    [Serializable]
    public class EventStreamDescriptor
    {
        public EventStreamDescriptor()
        {

        }

        public EventStreamDescriptor(string? aggregateType, Address address)
        {
            AggregateType = aggregateType;
            Address = address;
        }

        public string StreamIdentifier => $"{AggregateType}-{Address}";

        private string? AggregateType { get; set; }
        private Address? Address { get; set; }
    }
}

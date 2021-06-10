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

        public string StreamIdentifier => $"{AggregateType}-{Address.Value}";

        private string? AggregateType { get; set; }
        private Address? Address { get; set; }
    }
}

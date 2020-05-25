using System;

namespace Radix
{
    public class Aggregate<TCommand, TEvent>
        where TEvent : Event where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        public Aggregate(Address address, Send<TCommand, TEvent> accept)
        {
            Address = address;
            Accept = accept;

        }

        public Address Address { get; }
        public Send<TCommand, TEvent> Accept { get; }
    }
}

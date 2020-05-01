using System;

namespace Radix
{
    public class Aggregate<TCommand, TEvent>
        where TEvent : Event where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        public Aggregate(Address address, Send<TCommand, TEvent> send)
        {
            Address = address;
            Send = send;

        }

        public Address Address { get; }
        public Send<TCommand, TEvent> Send { get; }
    }
}

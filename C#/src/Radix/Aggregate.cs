using System;

namespace Radix
{
    /// <summary>
    /// Represent an aggregate root which can accept commands to process
    /// </summary>
    /// <typeparam name="TCommand">The type of command the aggregate can accept</typeparam>
    /// <typeparam name="TEvent">The type of events the aggregate can produce</typeparam>
    public class Aggregate<TCommand, TEvent>
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="accept"></param>
        public Aggregate(Address address, Accept<TCommand, TEvent> accept)
        {
            Address = address;
            Accept = accept;

        }

        public Address Address { get; }
        public Accept<TCommand, TEvent> Accept { get; }
    }
}

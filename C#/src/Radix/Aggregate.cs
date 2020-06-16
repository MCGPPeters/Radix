using System;

namespace Radix
{
    /// <summary>
    ///     Represent an aggregate root which can accept commands to process
    /// </summary>
    /// <typeparam name="TCommand">The type of command the aggregate can accept</typeparam>
    /// <typeparam name="TEvent">The type of events the aggregate can produce</typeparam>
    public class Aggregate<TCommand, TEvent>
        where TCommand : IComparable, IComparable<TCommand>, IEquatable<TCommand>
    {
        /// <summary>
        /// An aggregate instance should only be created by the runtime
        /// </summary>
        /// <param name="address"></param>
        /// <param name="accept"></param>
        internal Aggregate(Address address, Accept<TCommand, TEvent> accept)
        {
            Address = address;
            Accept = accept;
        }

        /// <summary>
        /// The address of the aggregate
        /// </summary>
        public Address Address { get; }

        /// <summary>
        /// Accepts commands and returns either the resulting events or the errors that occured
        /// </summary>
        public Accept<TCommand, TEvent> Accept { get; }
    }
}

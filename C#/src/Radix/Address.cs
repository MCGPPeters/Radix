using System;

namespace Radix
{
    /// <summary>
    /// A reference to and identifier of an aggregate
    /// </summary>
    public class Address : Value<Guid>
    {
        /// <summary>
        /// Creates an aggregate with a system generated GUID
        /// </summary>
        public Address() : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Creates an aggregate with the specific GUID
        /// </summary>
        /// <param name="guid"></param>
        public Address(Guid guid) => Value = guid;

        /// <summary>
        /// A guid representation
        /// </summary>
        public Guid Value { get; }

        public override string ToString() => Value.ToString();

        public static implicit operator Address(Guid guid) => new Address(guid);

    }
}

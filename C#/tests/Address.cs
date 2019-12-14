using System;

namespace Radix.Tests
{
    public readonly struct Address : Value<Guid>
    {
        public Address(Guid guid)
        {
            Value = guid;
        }

        public Guid Value { get; }

        public static implicit operator Address(Guid guid)
        {
            return new Address(guid);
        }
    }
}
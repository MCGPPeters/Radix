using System;

namespace Radix
{
    public readonly struct Address : Value<Guid>
    {
        public Address(Guid guid) => Value = guid;

        public Guid Value { get; }

        public static implicit operator Address(Guid guid) => new Address(guid);

        public override string ToString() => Value.ToString();
    }
}

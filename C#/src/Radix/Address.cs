using System;

namespace Radix
{
    public class Address : Value<Guid>
    {
        public Address()
        {

        }

        public Address(Guid guid) => Value = guid;

        public Guid Value { get; } = Guid.NewGuid();

        public override string ToString() => Value.ToString();

        public static implicit operator Address(Guid guid) => new Address(guid);
    }
}

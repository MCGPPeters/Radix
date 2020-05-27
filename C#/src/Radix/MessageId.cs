using System;

namespace Radix
{
    public readonly struct MessageId : Value<Guid>
    {
        public MessageId(Guid value) => Value = value;

        public Guid Value { get; }
    }

}

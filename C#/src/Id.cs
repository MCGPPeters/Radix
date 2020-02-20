using System;
using Radix;

namespace Aquila.Presentation.UI.WPF.Shell
{
    public readonly struct Id : Value<Guid>
    {
        public Id(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }

        public static bool operator ==(Id left, Id right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Id left, Id right)
        {
            return !(left == right);
        }

        public static implicit operator Id(Guid guid)
        {
            return new Id(guid);
        }

        public static implicit operator Guid(Id id)
        {
            return id.Value;
        }
    }
}

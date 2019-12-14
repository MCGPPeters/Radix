using Radix;
using System;

namespace Aquila.Presentation.UI.WPF.Shell
{
    public readonly struct Id : Value<Guid>
    {
        public Id(Guid value) => Value = value;

        public Guid Value { get; }

        public static bool operator ==(Id left, Id right) => left.Equals(right);

        public static bool operator !=(Id left, Id right) => !(left == right);

        public static implicit operator Id(Guid guid) => new Id(guid);

        public static implicit operator Guid(Id id) => id.Value;
    }
}
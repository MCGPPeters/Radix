using System;

namespace Radix.Tests
{
    public readonly struct Version : IVersion, IComparable<Version>
    {

        private Version(long value)
        {
            Value = value;

        }

        private long Value { get; }

        public int CompareTo(Version other)
        {
            return Value.CompareTo(other);
        }

        public static implicit operator Version(long value)
        {
            return new Version(value);
        }

        public static implicit operator long(Version version)
        {
            return version.Value;
        }
    }
}
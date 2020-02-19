using System;

namespace Radix
{
    public readonly struct Version : IVersion
    {

        public Version(long value)
        {
            Value = value;
        }

        public long Value { get; }

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
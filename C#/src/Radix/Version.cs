using System;

namespace Radix
{
    public abstract record Version(long Value) : Alias<long>(Value), IComparable<Version>
    {
        public int CompareTo(Version other) => Value.CompareTo(other.Value);
    }
}

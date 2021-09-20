namespace Radix;

public abstract record Version(long Value) : IComparable<Version>
{
    public int CompareTo(Version? other) => Value.CompareTo(other?.Value);

}

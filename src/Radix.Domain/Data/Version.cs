using Radix.Data;

namespace Radix.Domain.Data;

public abstract record Version(long Value) : IComparable<Version>, Order<Version>
{
    public int CompareTo(Version? other) => Value.CompareTo(other?.Value);

    public static Func<Version, Version, bool> Equal { get; } = (left, right) => left.Value == right.Value;
    public static Func<Version, Version, bool> NotEqual { get; } = (left, right) => !Equal(left, right);
    public static Func<Version, Version, Ordering> Compare { get; } = (left, right) =>
    {
        if (left < right) return new LT();
        if(right > left) return new GT();
        return new EQ();
    };

    public static bool operator <(Version left, Version right) => left.Value < right.Value;

    public static bool operator >(Version left, Version right) => left.Value > right.Value;

    public static bool operator >=(Version left, Version right) => left.Value >= right.Value;

    public static bool operator <=(Version left, Version right) => left.Value <= right.Value;
}

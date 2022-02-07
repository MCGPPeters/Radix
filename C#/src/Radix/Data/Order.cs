namespace Radix.Data;

public interface Order<in T> : Equality<T>
    where T : Order<T>
{
    static abstract Func<T, T, Ordering> Compare { get; }

    static abstract bool operator <(T left, T right);

    static abstract bool operator >(T left, T right);

    static abstract bool operator >=(T left, T right);

    static abstract bool operator <=(T left, T right);
}

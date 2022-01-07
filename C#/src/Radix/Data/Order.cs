namespace Radix.Data;

public interface Order<in T> : Equality<T>
{
    static abstract Func<T, T, Ordering> Compare { get; }
}

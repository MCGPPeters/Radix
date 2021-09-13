namespace Radix.Data;

public interface Order<T> : Equality<T>
{
    static abstract Func<T, T, Ordering> Compare { get; }
}

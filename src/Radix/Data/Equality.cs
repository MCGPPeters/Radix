namespace Radix.Data;

public interface Equality<in T>
{
    static abstract Func<T, T, bool> Equal { get; }
    static abstract Func<T, T, bool> NotEqual { get; }
}

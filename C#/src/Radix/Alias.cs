namespace Radix;

public interface Alias<T, TTarget> where T : Alias<T, TTarget>
{
    static abstract implicit operator T(TTarget alias);
    static abstract implicit operator TTarget(T t);
}

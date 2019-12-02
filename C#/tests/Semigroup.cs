namespace Radix.Tests
{
    public interface Semigroup<T>
    {
        T Append(T x, T y);
    }
}
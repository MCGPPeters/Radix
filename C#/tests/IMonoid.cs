namespace Radix.Tests
{
    public interface IMonoid<T>
    {
        T Identity { get; }

        T Combine(T t);

        T Concat(IMonoid<T> other);
    }
}

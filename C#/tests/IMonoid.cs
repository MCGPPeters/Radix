namespace Radix.Tests
{
    public interface IMonoid<T>
    {
        T Identity { get; }

        T Append(T t);

        T Concat(IMonoid<T> other);
    }
}

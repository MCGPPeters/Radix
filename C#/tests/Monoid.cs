namespace Radix.Tests
{
    public abstract class Monoid<T> : IMonoid<T>
    {
        public abstract T Identity { get; }

        public abstract T Combine(T t);

        public T Concat(IMonoid<T> other)
        {
            return Combine(other.Identity);
        }

        public static T operator +(Monoid<T> first, Monoid<T> second)
        {
            return first.Concat(second);
        }
    }
}

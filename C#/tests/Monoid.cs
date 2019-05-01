namespace Radix.Tests
{
    public abstract class Monoid<T> : IMonoid<T>
    {
        public abstract T Identity { get; }

        public abstract T Append(T t);

        public T Concat(IMonoid<T> other)
        {
            return Append(other.Identity);
        }

        public static T operator +(Monoid<T> first, Monoid<T> second)
        {
            return first.Concat(second);
        }
    }
}

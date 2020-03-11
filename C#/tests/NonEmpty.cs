using System.Collections;
using System.Collections.Generic;

namespace Radix.Tests
{
    /// <summary>
    ///     A collection that contains at least 1 value.
    ///     That value can be null
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface NonEmpty<out T>
    {
    }

    /// <summary>
    ///     A single value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class Singleton<T> : NonEmpty<T>
    {
        public Singleton(T value)
        {
            Value = value;
        }

        public T Value { get; }
    }

    internal class NonEmptyList<T> : NonEmpty<T>, IEnumerable<T>
    {
        public NonEmptyList(T head, params T[] tail)
        {
            Head = head;
            Tail = tail;
        }

        public T Head { get; }
        public T[] Tail { get; }

        public IEnumerator<T> GetEnumerator()
        {
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return null;
        }
    }
}

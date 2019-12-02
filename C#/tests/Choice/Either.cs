using System;

namespace Radix.Tests.Choice
{
    public struct Either<T, U> : Choice<T, U>
    {
        internal Either(T t)
        {
            if (t is object) Value = t;
            else
                throw new ArgumentNullException(nameof(t));

        }

        public static implicit operator Either<T, U>(T t)
        {
            return new Either<T, U>(t);
        }

        public static implicit operator T(Either<T, U> either)
        {
            return either.Value;
        }

        public T Value { get; }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="value"></param>
        public void Deconstruct(out T value)
        {
            value = Value;
        }
    }
}

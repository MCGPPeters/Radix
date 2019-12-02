using System;

namespace Radix.Tests.Validated
{

    public struct Valid<T> : Validated<T>
    {
        internal Valid(T t)
        {
            if (t is object) Value = t;
            else
                throw new ArgumentNullException(nameof(t));

        }

        public static implicit operator Valid<T>(T t)
        {
            return new Valid<T>(t);
        }

        public static implicit operator T(Valid<T> ok)
        {
            return ok.Value;
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

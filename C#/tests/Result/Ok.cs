using System;

namespace Radix.Tests.Result
{

    public struct Ok<T> : Result<T>
    {
        internal Ok(T t)
        {
            if (t is object) Value = t;
            else
                throw new ArgumentNullException(nameof(t));

        }

        public static implicit operator Ok<T>(T t)
        {
            return new Ok<T>(t);
        }

        public static implicit operator T(Ok<T> ok)
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

using System;

namespace Radix.Result
{

    public readonly struct
        Ok<T, TError> : Result<T, TError>
    {
        internal Ok(T t)
        {
            if (t is object)
            {
                Value = t;
            }
            else
            {
                throw new ArgumentNullException(nameof(t));
            }

        }

        public static implicit operator Ok<T, TError>(T t) => new Ok<T, TError>(t);

        public static implicit operator T(Ok<T, TError> ok) => ok.Value;

        public T Value { get; }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="value"></param>
        public void Deconstruct(out T value) => value = Value;
    }

}

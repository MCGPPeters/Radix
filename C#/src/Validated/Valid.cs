using System;

namespace Radix.Validated
{

    public readonly struct Valid<T> : Validated<T>
    {
        internal Valid(T t)
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

        public T Value { get; }

        public static implicit operator Valid<T>(T t) => new Valid<T>(t);

        public static implicit operator T(Valid<T> ok) => ok.Value;


        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="value"></param>
        public void Deconstruct(out T value) => value = Value;
    }

}

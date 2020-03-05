using System;

namespace Radix.Option
{

    public readonly struct Some<T> : Option<T>
    {
        internal T Value { get; }

        internal Some(T value)
        {
            if (value is object)
                Value = value;
            else
                throw new ArgumentNullException(nameof(value));
        }

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

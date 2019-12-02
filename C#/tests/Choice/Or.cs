using System;

namespace Radix.Tests.Choice
{
    public struct Or<T, U> : Choice<T, U>
    {
        internal Or(U u)
        {
            if (u is object) Value = u;
            else
                throw new ArgumentNullException(nameof(u));

        }

        public static implicit operator Or<T, U>(U u)
        {
            return new Or<T, U>(u);
        }

        public static implicit operator U(Or<T, U> or)
        {
            return or.Value;
        }

        public U Value { get; }

        /// <summary>
        ///     Type deconstructor, don't remove even though no references are obvious
        /// </summary>
        /// <param name="value"></param>
        public void Deconstruct(out U value)
        {
            value = Value;
        }
    }
}

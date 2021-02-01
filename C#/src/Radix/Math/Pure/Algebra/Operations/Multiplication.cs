using System;

namespace Radix.Math.Pure.Algebra.Operations
{
    public class Multiplication<T> : Binary<T>
    {
        public Multiplication(Func<T, T, T> apply)
        {
            Apply = apply;
        }

        public Func<T, T, T> Apply { get; }
    }
}

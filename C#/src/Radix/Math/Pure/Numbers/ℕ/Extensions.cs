using System;
using Radix.Math.Pure.Algebra.Operations;

namespace Radix.Math.Pure.Numbers.ℕ
{
    public static class Extensions
    {
        public static readonly Addition<Natural> Add = new(
            (a, b) =>
            {
                 static Natural Add(Natural x, Natural y)
                     => (x, y) switch
                     {
                         (Zero, _) => y,
                         (_, Zero) => x,
                         (Successor d, Successor e) => new Successor (Add(d, e)),
                         _ => throw new ArgumentOutOfRangeException()
                     };
                 return Add(a, b);
            });

        public static readonly Multiplication<Natural> Multiply = new(
            (a, b) =>
            {
                 static Natural Multiply(Natural x, Natural y)
                     => (x, y) switch
                     {
                         (Zero, _) => new Zero(),
                         (_, Zero) => new Zero(),
                         (Successor d, Successor e) => new Successor (Multiply(d, e)),
                         _ => throw new ArgumentOutOfRangeException()
                     };
                 return Multiply(a, b);
            });

    }
}

using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Math.Pure.Numbers.ℕ;

public class Multiplication : Monoid<Natural>
{
    public static Natural Identity => new Zero();

    public static Func<Natural, Natural, Natural> Combine =>
       (a, b) =>
       {
           static Natural Multiply(Natural x, Natural y) => (x, y) switch
           {
               (Zero, _) => new Zero(),
               (_, Zero) => new Zero(),
               (Successor d, Successor e) => new Successor(Multiply(d, e)),
               _ => throw new ArgumentOutOfRangeException()
           };

           return Multiply(a, b);
       };
    }

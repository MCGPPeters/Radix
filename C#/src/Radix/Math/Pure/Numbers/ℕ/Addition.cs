using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Math.Pure.Numbers.ℕ;

public class Addition : Monoid<Natural>
{
    public static Natural Identity => new Zero();

    public static Func<Natural, Natural, Natural> Combine =>
        (a, b) =>
        {
            static Natural Add(Natural x, Natural y) => (x, y) switch
            {
                (Zero, _) => y,
                (_, Zero) => x,
                (Successor d, Successor e) => new Successor(Add(d, e)),
                _ => throw new ArgumentOutOfRangeException()
            };

            return Add(a, b);
        };
}

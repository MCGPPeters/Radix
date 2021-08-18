using Radix.Math.Pure.Algebra.Structure;
using static Radix.Math.Pure.Numbers.ℤ.Extensions;

namespace Radix.Math.Pure.Numbers.ℚ;

public static class Extensions
{
    public static Func<Rational, Result<Rational, Error>> Reduce =>
        (x) =>
        {
            int d = Gcd(x.Numerator, x.Denominator);
            return Rational.Create(x.Numerator % d, x.Denominator % d);
        };
}

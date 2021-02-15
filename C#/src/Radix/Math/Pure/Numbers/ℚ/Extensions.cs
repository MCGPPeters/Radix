using System;
using Radix.Result;
using static Radix.Result.Extensions;
using static Radix.Math.Pure.Numbers.ℤ.Extensions;

namespace Radix.Math.Pure.Numbers.ℚ
{
    public static class Extensions
    {
        private static Func<Rational, Result<Rational, Error>> Reduce =>
            (x) =>
            {
                var d = Gcd(x.Numerator, x.Denominator);
                return Rational.Create(x.Numerator % d, x.Denominator % d);
            };
    }
}

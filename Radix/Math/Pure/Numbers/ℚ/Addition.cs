using Radix.Math.Pure.Algebra.Structure;
using static Radix.Math.Pure.Numbers.ℤ.Extensions;

namespace Radix.Math.Pure.Numbers.ℚ;

public class Addition : Field<Rational>
{
    public static Rational Identity => new(0, 1);

    public static Func<Rational, Rational, Rational> Combine => (x, y) =>
    {
        switch (x.Denominator == y.Denominator)
        {
            case true:
                return new Rational(x.Numerator + y.Numerator, x.Denominator);
            case false:
                {
                    int lcm = Lcm(x.Denominator, y.Denominator);
                    int xNumerator = lcm / x.Denominator * x.Numerator;
                    int yNumerator = lcm / y.Denominator * y.Numerator;

                    return new Rational(xNumerator + yNumerator, x.Denominator);
                }
        }
    };

    public static Func<Rational, Rational> Invert => a => new(-a.Numerator, a.Denominator);
}

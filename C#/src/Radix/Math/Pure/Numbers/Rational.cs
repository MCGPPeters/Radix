using Radix.Math.Pure.Numbers.ℚ;
using static Radix.Math.Pure.Numbers.ℤ.Extensions;

namespace Radix.Math.Pure.Numbers;

public record Rational
{

    internal Rational(int numerator, int denominator)
    {
        Numerator = numerator;
        Denominator = denominator;
    }

    public static Result<Rational, Error> Create(int numerator, int denominator)
        => denominator switch
        {
            > 0 => Ok<Rational, Error>(new Rational(numerator, denominator)),
            _ => Error<Rational, Error>($"The {nameof(denominator)} must be greater then zero")
        };

    public int Numerator { get; }

    public int Denominator { get; }

    public static Rational operator *(Rational x, Rational y) =>
        Multiplication.Combine(x, y);

    public static Rational operator -(Rational x, Rational y)
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

                    return new Rational(xNumerator - yNumerator, x.Denominator);
                }
        };
    }

    public static Rational operator /(Rational x, Rational y)
    {
        Rational inverse = Multiplication.Invert(y);
        return Multiplication.Combine(x, inverse);
    }

    public static Rational operator +(Rational x, Rational y) =>
        Addition.Combine(x, y);

    /// <summary>
    /// Additive inverse
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static Rational operator -(Rational x) =>
        Addition.Invert(x);
}

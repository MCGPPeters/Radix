﻿using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Math.Pure.Numbers.ℚ;

public class Multiplication : Group<Rational>, Algebra.Operations.Multiplication
{
    public static Rational Identity => new(1, 1);

    public static Func<Rational, Rational, Rational> Combine => (x, y) => new(x.Numerator * y.Numerator, x.Denominator * y.Denominator);

    public static Func<Rational, Rational> Invert => a => new(a.Denominator, a.Numerator);

    public static Func<Rational, Rational, Rational> Divide => (x, y) => Combine(x, Invert(y));
}

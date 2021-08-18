using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Math.Pure.Numbers.ℝ;

public class Multiplication : Field<double>
{
    public static double Identity => 1.0;

    public static Func<double, double, double> Combine => new((x, y) => x * y);

    public static Func<double, double> Invert => a => 1/a;
}

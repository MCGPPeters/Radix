using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Math.Pure.Numbers.ℝ;

public class Addition : Field<double>
{
    public static double Identity => 0.0;

    public static Func<double, double, double> Combine => (x, y) => x + y;

    public static Func<double, double> Invert => a => -a;
}

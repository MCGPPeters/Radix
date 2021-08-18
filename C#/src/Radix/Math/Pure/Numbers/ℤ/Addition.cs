using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Math.Pure.Numbers.ℤ;

public class Addition : Group<int>
{
    public static int Identity => 0;

    public static Func<int, int, int> Combine => (x, y) => x + y;

    public static Func<int, int> Invert => a => -a;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Math.Pure.Numbers;

namespace Radix.Math.Pure.Numbers;

[Alias<int>]
public partial struct Integer : Order<Integer>, Number
{
    public static Func<Integer, Integer, Ordering> Compare => (x, y) =>
        Comparer<double>.Default.Compare(x, y) switch
        {
            < 0 => new LT(),
            0 => new EQ(),
            _ => new GT()
        };

    public static Func<Integer, Integer, bool> Equal => (x, y)
        => Compare(x, y) == new EQ();

    public static Func<Integer, Integer, bool> NotEqual => (x, y)
        => !Equal(x, y);

    public static bool operator <(Integer left, Integer right) =>
        Compare(left, right) == new LT();

    public static bool operator >(Integer left, Integer right) =>
        Compare(left, right) == new GT();

    public static bool operator <=(Integer left, Integer right) =>
        left < right || Equal(left, right);
    public static bool operator >=(Integer left, Integer right) =>
        left > right || Equal(left, right);
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Data;

namespace Radix.Math.Pure.Numbers.ℤ
{
    public class Order : Order<int>
    {
        public static Func<int, int, Ordering> Compare => (x, y) =>
            Comparer<int>.Default.Compare(x, y) switch
            {
                < 0 => new LT(),
                0 => new EQ(),
                > 0 => new GT()
            };

        public static Func<int, int, bool> Equal => (x, y) => EqualityComparer<int>.Default.Equals(x, y);

        public static Func<int, int, bool> NotEqual => (x, y) => !Equal(x, y);
    }
}

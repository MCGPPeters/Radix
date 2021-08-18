using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Data.String
{
    public class Concat : Monoid<string>
    {
        public static string Identity => "";

        public static Func<string, string, string> Combine => (x, y) => x + y;
    }
}

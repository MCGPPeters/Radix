using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix
{
    public static class Prelude<T>
    {
        public static Func<T, T> Id => x => x;
    }
}

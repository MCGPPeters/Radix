using System;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Math.Applied.Optimization
{
    public static class Selection
    {
        public static Func<Func<T, U>, IEnumerable<T>, U?> ArgMax<T, U>() => (f, xs) => xs.Max(f);
    }
}

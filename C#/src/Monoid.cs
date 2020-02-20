using System.Collections.Generic;
using System.Linq;

namespace Radix
{

    public interface Monoid<T> : Semigroup<T>
    {
        T Empty();

        T Concat(IEnumerable<T> xs)
        {
            return xs.Aggregate(Empty(), Append);
        }

        T Concat(params T[] xs)
        {
            return xs.Aggregate(Empty(), Append);
        }
    }

}

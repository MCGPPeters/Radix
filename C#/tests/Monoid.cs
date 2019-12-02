using System.Collections.Generic;
using System.Linq;

namespace Radix.Tests
{

    public interface Monoid<T> : Semigroup<T>
    {
        T Empty();

        T Concat(IEnumerable<T> xs) =>
            xs.Aggregate(Empty(), Append);

        T Concat(params T[] xs) =>
            xs.Aggregate(Empty(), Append);
        
        
    }

}

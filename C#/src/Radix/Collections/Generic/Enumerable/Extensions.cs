using System;
using System.Collections.Generic;

namespace Radix.Collections.Generic.Enumerable
{
    public static class Extensions
    {
        public static T MaxBy<T, U>(this IEnumerable<T> enumerable, Func<T, U> projection)
            => enumerable.
                Aggregate((current, next) =>
                    projection(current) > projection(next)
                        ? current
                        : next)
                .First();
    }

    ///    let inline maxBy (projection) (list: NonEmpty<_>) =
        ///list |> fold (fun x y -> if projection x > projection y then x else y) (list |> head)
}

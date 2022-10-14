using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Data.Collections.Generic.List;

public static class Extensions
{
    public static T ElementAt<T>(this List<T> list, int n)
        where T : notnull =>
        list switch
        {
            EmptyList<T> _ => throw new ArgumentException("The empty list contains no items"),
            NonEmptyList<T> l => n switch
            {
                < 0 => throw new ArgumentException("n must be greater than zero"),
                0 => l.Head,
                _ => l.Tail.ElementAt(n - 1)
            },
            _ => throw new NotImplementedException(),
        };
}

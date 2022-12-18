using Radix.Data;
using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Data.Collections.Generic.Enumerable;

public static class Extensions
{
    /// <summary>
    ///     Generates a sequence of evenly spaced points in the closed interval [start,stop].
    /// </summary>
    public static IEnumerable<double> Sequence(double start, double stop, int length = 100)
    {
        double step = (start - stop) / (length - 1);

        double[]? data = new double[length];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = start + i * step;
        }

        data[data.Length - 1] = stop;
        return data;
    }

    /// <summary>
    /// Insertion sort
    /// </summary>
    /// <param name="xs"></param>
    /// <returns></returns>
    public static T[] Sort<T>(this T[] xs)
        where T : Order<T>
    {
        int n = xs.Length;
        for (int i = 1; i < n; i++)
        {
            for (int j = i; j > 0; j--)
            {
                switch (T.Compare(xs[j], xs[j - 1]))
                {
                    case LT:
                        T swap = xs[j];
                        xs[j] = xs[j - 1];
                        xs[j - 1] = swap;
                        break;
                }
            }
        }
        return xs;
    }


    public static T Aggregate<T>(this IEnumerable<T> xs) where T : Monoid<T> => xs.Aggregate(T.Identity, T.Combine);

    public static T Fold<T>(this IEnumerable<T> xs) where T: Monoid<T> => xs.Aggregate<T>();

    public static T Sum<T>(this IEnumerable<T> values) where T: Monoid<T>, Addition => Aggregate<T>(values);
    public static T Σ<T>(this IEnumerable<T> values) where T : Monoid<T>, Addition => Sum<T>(values);

    public static T Product<T>(this IEnumerable<T> values) where T : Monoid<T>, Multiplication => Aggregate<T>(values);
    public static T Π<T>(this IEnumerable<T> values) where T : Monoid<T>, Multiplication => Product<T>(values);
}

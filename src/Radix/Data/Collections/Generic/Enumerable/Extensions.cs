using Radix.Data;
using Radix.Math.Pure.Algebra.Operations;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Data.Collections.Generic.Enumerable;

public static class Extensions
{
    /// <summary>
    ///     Generates a sequence of evenly spaced points in the closed interval [start,stop].
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="range"></param>
    /// <returns></returns>
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
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TOrd"></typeparam>
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


    public static T Aggregate<T, M>(this IEnumerable<T> xs) where M : Monoid<T> => xs.Aggregate(M.Identity, M.Combine);

    public static T Fold<T, M>(this IEnumerable<T> xs) where M : Monoid<T> => xs.Aggregate<T, M>();

    public static T Sum<T, M>(this IEnumerable<T> values) where M : Monoid<T>, Addition => Aggregate<T, M>(values);
    public static T Σ<T, M>(this IEnumerable<T> values) where M : Monoid<T>, Addition => Sum<T, M>(values);

    public static T Product<T, M>(this IEnumerable<T> values) where M : Monoid<T>, Multiplication => Aggregate<T, M>(values);
    public static T Π<T, M>(this IEnumerable<T> values) where M : Monoid<T>, Multiplication => Product<T, M>(values);


    //public static T Average<T, GAdd, GMul>(this IEnumerable<T> values)
    //    where GAdd : Group<T>, Addition
    //    where GMul : Group<T>, Multiplication
    //    where T : IFloatingPoint<T>
    //    => GMul.Combine(Sum<T, GAdd>(values), GMul.Invert(T.Create(values.Count())));

    //public static T StandardDeviation<T, GAdd, GMul>(IEnumerable<T> values)
    //    where GAdd : Group<T>, Addition
    //    where GMul : Group<T>, Multiplication
    //    where T : IFloatingPoint<T>
    //{
    //    T standardDeviation = GAdd.Identity;

    //    if (values.Any())
    //    {
    //        T average = Average<T, GAdd, GMul>(values);
    //        T variance = Average<T, GAdd, GMul>(values.Select((value) =>
    //        {
    //            T deviation = GAdd.Combine(value, GAdd.Invert(average));
    //            return GMul.Combine(deviation, deviation);
    //        }));
    //        standardDeviation = T.Sqrt(variance);
    //    }

    //    return standardDeviation;
    //}

}

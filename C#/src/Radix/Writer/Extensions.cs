using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Writer;

public static class Extensions
{
    public static Writer<R, TOutput, Monoid> Bind<T, R, TOutput, Monoid>(this Writer<T, TOutput, Monoid> writer, Func<T, Writer<R, TOutput, Monoid>> f)
        where Monoid : Monoid<TOutput>
    {
        (T? t, TOutput? o1) = writer;
        (R? r, TOutput? o2) = f(t);
        return f(t) with { output = Monoid.Combine(o1, o2) };
    }


    public static Writer<R, TOutput, Monoid> Map<T, R, TOutput, Monoid>(this Writer<T, TOutput, Monoid> writer, Func<T, R> f)
        where Monoid : Monoid<TOutput>
    {
        (T? t, TOutput? output) = writer;
        R? result = f(t);
        return new Writer<R, TOutput, Monoid>(result, output);
    }

    public static Writer<TProjection, TOutput, Monoid> SelectMany<T, R, TOutput, TProjection, Monoid>(this Writer<T, TOutput, Monoid> writer, Func<T, Writer<R, TOutput, Monoid>> f,
        Func<T, R, TProjection> project)
            where Monoid : Monoid<TOutput>
        {
            (T? t, TOutput? output) = writer;
            return f(t).Bind(r => Return<TProjection, TOutput, Monoid>(project(t, r)));
        }

    public static Writer<R, TOutput, Monoid> Select<T, R, TOutput, Monoid>(this Writer<T, TOutput, Monoid> writer, Func<T, R> f)
        where Monoid : Monoid<TOutput> =>
            writer.Map(f);

    public static Writer<T, TOutput, Monoid> Return<T, TOutput, Monoid>(this T t)
        where Monoid : Monoid<TOutput> =>
            new(t, Monoid.Identity);

    public static Writer<T, TOutput, Monoid> Return<T, TOutput, Monoid>(this T t, TOutput output)
        where Monoid : Monoid<TOutput> =>
            new (t, output);

}


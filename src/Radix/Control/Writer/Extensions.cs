using Radix.Data;
using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Control.Writer;

public static class Extensions
{
    public static Writer<R, TOutput> Bind<T, R, TOutput>(this Writer<T, TOutput> writer, Func<T, Writer<R, TOutput>> f)
        where TOutput : Monoid<TOutput>
    {
        (T? t, TOutput? o1) = writer;
        (R? r, TOutput? o2) = f(t);
        return f(t) with { output = TOutput.Combine(o1, o2) };
    }


    public static Writer<R, TOutput> Map<T, R, TOutput>(this Writer<T, TOutput> writer, Func<T, R> f)
        where TOutput : Monoid<TOutput>
    {
        (T? t, TOutput? output) = writer;
        R? result = f(t);
        return new Writer<R, TOutput>(result, output);
    }

    public static Writer<TProjection, TOutput> SelectMany<T, R, TOutput, TProjection>(this Writer<T, TOutput> writer, Func<T, Writer<R, TOutput>> f,
        Func<T, R, TProjection> project)
            where TOutput : Monoid<TOutput>
    {
        (T? t, TOutput? output) = writer;
        return f(t).Bind(r => Return<TProjection, TOutput>(project(t, r)));
    }

    public static Writer<R, TOutput> Select<T, R, TOutput>(this Writer<T, TOutput> writer, Func<T, R> f)
        where TOutput : Monoid<TOutput> =>
            writer.Map(f);

    public static Writer<T, TOutput> Return<T, TOutput>(this T t)
        where TOutput : Monoid<TOutput> =>
            new(t, TOutput.Identity);

    public static Writer<T, TOutput> Return<T, TOutput>(this T t, TOutput output)
        where TOutput : Monoid<TOutput> =>
            new(t, output);

}


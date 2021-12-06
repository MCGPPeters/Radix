using Radix.Math.Pure.Algebra.Structure;

namespace Radix.Writer;

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

    public static Writer<T, TOutput> Return<T, TOutput>(T t)
        where TOutput : Monoid<TOutput> =>
            new(t, TOutput.Identity);

}


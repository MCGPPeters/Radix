namespace Radix.Control.Builder;

public static class Extensions
{
    public static Builder<TResult> Select<T, TResult>(this Builder<T> builder, Func<T, TResult> f)
    {
        var newItem = f(builder.Build());
        return new Builder<TResult>(newItem);
    }
}

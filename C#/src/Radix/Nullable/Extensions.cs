using Radix.Option;

namespace Radix.Nullable
{
    public static class Extensions
    {
        public static TResult? Map<T, TResult>
            (this T? nullable, Func<T, TResult> f) =>
                nullable is not null
                ? f(nullable)
                : default;

        public static Option<T> AsOption<T>(this T? nullable) =>
            nullable is not null
            ? new Some<T>(nullable)
            : None<T>();

    }

}

using Radix.Data;

using static Radix.Control.Option.Extensions;

namespace Radix.Control.Nullable
{
    public static class Extensions
    {
        public static TResult? Map<T, TResult>
            (this T? nullable, Func<T, TResult> f) =>
                nullable is not null
                ? f(nullable)
                : default;

#pragma warning disable CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.
        public static Option<T> AsOption<T>(this T? nullable) =>
            nullable is not null
            ? new Some<T>(nullable)
            : None<T>();
#pragma warning restore CS8714 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'notnull' constraint.

    }

}

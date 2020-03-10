using System;

namespace Radix.Option
{
    public static class Extensions
    {
        public static Option<T> Some<T>(T value)
        {
            return new Some<T>(value); // wrap the given value into a Some
        }

        public static None<T> None<T>()
        {
            return Option.None<T>.Default;
        }

        public static Option<TResult> Bind<T, TResult>
            (this Option<T> option, Func<T, Option<TResult>> f)
        {
            return option switch
            {
                Some<T>(var t) => f(t),
                None<T> _ => None<TResult>(),
                _ => throw new NotSupportedException()
            };
        }

        public static Option<TResult> Map<T, TResult>
            (this Option<T> option, Func<T, TResult> f)
        {
            return option switch
            {
                Some<T>(var t) => Some(f(t)),
                None<T> _ => None<TResult>(),
                _ => throw new NotSupportedException()
            };
        }
    }
}
using System;

namespace Radix.Maybe
{
    public static class Extensions
    {
        public static Maybe<T> Some<T>(T value)
        {
            return new Some<T>(value); // wrap the given value into a Some
        }

        public static None<T> None<T>()
        {
            return Maybe.None<T>.Default;
        }

        public static Maybe<TResult> Bind<T, TResult>
            (this Maybe<T> maybe, Func<T, Maybe<TResult>> f)
        {
            return maybe switch
            {
                Some<T>(var t) => f(t),
                None<T> _ => None<TResult>(),
                _ => throw new NotSupportedException()
            };
        }

        public static Maybe<TResult> Map<T, TResult>
            (this Maybe<T> maybe, Func<T, TResult> f)
        {
            return maybe switch
            {
                Some<T>(var t) => Some(f(t)),
                None<T> _ => None<TResult>(),
                _ => throw new NotSupportedException()
            };
        }
    }
}

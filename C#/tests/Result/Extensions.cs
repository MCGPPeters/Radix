using System;

namespace Radix.Tests.Result
{
    public static class Extensions
    {
        public static Result<T> Ok<T>(T t)
        {
            return new Ok<T>(t);
        }

        public static Result<TResult> Bind<T, TResult>(this Result<T> result, Func<T, Result<TResult>> function)
        {
            return result switch
                {
                Ok<T> (var value) => function(value),
                Error<T> (var messages) => new Error<TResult>(messages),
                _ => throw new InvalidOperationException("Unlikely")
                };
        }
    }
}

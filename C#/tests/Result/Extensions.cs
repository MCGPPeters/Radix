using System;

namespace Radix.Tests.Result
{
    public static class Extensions
    {
        public static Result<T, TError> Ok<T, TError>(T t)
        {
            return new Ok<T, TError>(t);
        }

        public static Result<T, TError> Error<T, TError>(TError error)
        {
            return new Error<T, TError>(error);
        }

        public static Result<TResult, TError> Bind<T, TResult, TError>(this Result<T, TError> result, Func<T, Result<TResult, TError>> function) 
            => result switch
                {
                    Ok<T, TError> (var value) => function(value),
                    Error<T, TError> (var messages) => Error<TResult, TError>(messages),
                    _ => throw new NotSupportedException("Unlikely")
                };

        public static Result<TResult, TError> Map<T, TResult, TError>(this Result<T, TError> result, Func<T, TResult> function) 
            => result.Bind(x => Ok<TResult, TError>(function(x)));

        public static Result<TResult, TError> Apply<T, TResult, TError>(this Result<Func<T, TResult>, TError> selector, Result<T, TError> result)
            => selector.Bind(f => result.Map(x => f(x)));
    }
}

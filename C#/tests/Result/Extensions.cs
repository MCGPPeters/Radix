using System;

namespace Radix.Tests.Result
{
    public static class Extensions
    {
        public static Result<T> Ok<T>(T t)
        {
            return new Ok<T>(t);
        }

        public static Result<T> Error<T>(params string[] message)
        {
            return new Error<T>();
        }

        public static Result<TResult> Bind<T, TResult>(this Result<T> result, Func<T, Result<TResult>> function) 
            => result switch
                {
                    Ok<T> (var value) => function(value),
                    Error<T> (var messages) => new Error<TResult>(messages),
                    _ => throw new InvalidOperationException("Unlikely")
                };

        public static Result<TResult> Map<T, TResult>(this Result<T> result, Func<T, TResult> function) 
            => result.Bind(x => Ok(function(x)));

        public static Result<TResult> Apply<T, TResult>(this Result<Func<T, TResult>> selector, Result<T> result)
            => selector.Bind(f => result.Map(x => f(x)));
    }
}
using System;
using Radix.Monoid;

namespace Radix.Result
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
        {
            return result switch
            {
                Ok<T, TError>(var value) => function(value),
                Error<T, TError>(var error) => Error<TResult, TError>(error),
                _ => throw new NotSupportedException("Unlikely")
            };
        }

        public static Result<TResult, TError> Map<T, TResult, TError>(this Result<T, TError> result, Func<T, TResult> function)
        {
            return result switch
            {
                Ok<T, TError>(var value) => Ok<TResult, TError>(function(value)),
                Error<T, TError>(var error) => Error<TResult, TError>(error),
                _ => throw new NotSupportedException("Unlikely")
            };
        }

        public static Result<TResult, TError> Select<T, TResult, TError>(this Result<T, TError> result, Func<T, TResult> function)
        {
            return Map(result, function);
        }


        public static Result<T, TErrorResult> MapError<T, TError, TErrorResult>(this Result<T, TError> result, Func<TError, TErrorResult> function)

        {
            return result switch
            {
                Ok<T, TError>(var value) => Ok<T, TErrorResult>(value),
                Error<T, TError>(var error) => Error<T, TErrorResult>(function(error)),
                _ => throw new NotSupportedException("Unlikely")
            };
        }

        public static Result<TResult, TError> Apply<T, TResult, TError>(this Result<Func<T, TResult>, TError> fResult, Result<T, TError> xResult)
        {
            return (fResult, xResult) switch
            {
                (Ok<Func<T, TResult>, TError>(var f), Ok<T, TError>(var x)) => Ok<TResult, TError>(f(x)),
                (Error<Func<T, TResult>, TError>(var error), Ok<T, TError>(_)) => Error<TResult, TError>(error),
                (Ok<Func<T, TResult>, TError>(_), Error<T, TError>(var error)) => Error<TResult, TError>(error),
                (Error<Func<T, TResult>, TError>(var error), Error<T, TError> _) => Error<TResult, TError>(error),
                _ => throw new NotSupportedException("Unlikely")
            };
        }
    }
}

using System;
using System.Threading.Tasks;
using Radix.Future;
using Radix.Monoid;
using Radix.Result;
using static Radix.Result.Extensions;

namespace Radix.Tests.TaskResult
{
    public static class Extensions
    {
        public static Task<Result<TResult, TError>> Map<T, TResult, TError>
            (this Task<Result<T, TError>> task, Func<T, TResult> f) where TError : Monoid<TError>
            => task.Select(result => result.Map(f));
        
        public static Task<Result<T, TErrorResult>> MapError<T, TError, TErrorResult>
            (this Task<Result<T, TError>> task, Func<TError, TErrorResult> f) where TError : Monoid<TError> where TErrorResult : Monoid<TErrorResult>
            => task.Select(result => result.MapError(f));

        public static Task<Result<TResult, TError>> Bind<T, TResult, TError>(this Task<Result<T, TError>> task, Func<T, Task<Result<TResult, TError>>> function) where TError : Monoid<TError>
            => task.SelectMany(result => result switch
                {
                    Ok<T, TError>(var value) => function(value),
                    Error<T, TError>(var error) => Task.FromResult(Error<TResult, TError>(error)),
                    _ => throw new NotSupportedException("Unlikely")
                });
    }
}
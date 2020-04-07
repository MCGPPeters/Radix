using System;
using System.Threading.Tasks;
using Radix.Monoid;
using Radix.Result;
using static Radix.Result.Extensions;

namespace Radix.TaskResult
{
    public static class Extensions
    {
        public static async Task<Result<TResult, TError>> Select<T, TResult, TError>
            (this Task<Result<T, TError>> taskResult, Func<T, TResult> f)
        {
            return await taskResult switch
            {
                Ok<T, TError>(var value) => Ok<TResult, TError>(f(value)),
                Error<T, TError>(var error) => Error<TResult, TError>(error),
                _ => throw new NotSupportedException("Unlikely")
            };
        }

        public static async Task<Result<TResult, TError>> Bind<T, TResult, TError>
            (this Task<Result<T, TError>> taskResult, Func<T, Task<Result<TResult, TError>>> f)
        {
            var result = await taskResult;
            return result switch
            {
                Ok<T, TError>(var value) => await f(value),
                Error<T, TError>(var error) => Error<TResult, TError>(error),
                _ => throw new NotSupportedException()
            };
        }

        public static async Task<Result<TProjected, TError>> SelectMany<T, TResult, TProjected, TError>(this Task<Result<T, TError>> taskResult,
            Func<T, Task<Result<TResult, TError>>> f, Func<T, TResult, TProjected> project)

        {
            var result = await taskResult;
            return result switch
            {
                Ok<T, TError>(var value) => await f(value).Select(x => project(value, x)),
                Error<T, TError>(var error) => Error<TProjected, TError>(error),
                _ => throw new NotSupportedException()
            };
        }
    }
}

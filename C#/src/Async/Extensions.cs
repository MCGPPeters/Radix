using System;
using System.Linq;
using System.Threading.Tasks;

namespace Radix.Async
{
    public static class Extensions
    {
        public static async Task<TResult> Select<T, TResult>
            (this Task<T> task, Func<T, TResult> f)
        {
            return f(await task);
        }

        public static Task<TResult> Map<T, TResult>
            (this Task<T> task, Func<AggregateException, TResult> faulted, Func<T, TResult> completed)
        {
            return task.ContinueWith(
                t =>
                    t.Status == TaskStatus.Faulted
                        ? faulted(t.Exception)
                        : completed(t.Result));
        }

        public static async Task<TResult> Bind<T, TResult>
            (this Task<T> task, Func<T, Task<TResult>> f)
        {
            return await f(await task);
        }

        public static Task<TResult> SelectMany<T, TResult>
            (this Task<T> task, Func<T, Task<TResult>> f)
        {
            return Bind(task, f);
        }

        public static async Task<TResult> SelectMany<T, TResult>
            (this Task task, Func<Unit, Task<T>> bind, Func<Unit, T, TResult> project)
        {
            await task;
            var r = await bind(Unit.Instance);
            return project(Unit.Instance, r);
        }

        /// <summary>
        ///     Try to execute the first task, otherwise use the fallback method
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="task"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        public static Task<T> Otherwise<T>
            (this Task<T> task, Func<Task<T>> fallback)
        {
            return task.ContinueWith(
                t => t.Status == TaskStatus.Faulted
                    ? fallback()
                    : Task.FromResult(t.Result)).Unwrap();
        }

        /// <summary>
        ///     Retry with delays as long as the task is in a faulted state.
        ///     The number of intervals also indicate the number of retries
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <param name="intervals">Delays between retries</param>
        /// <returns></returns>
        public static Task<T> Retry<T>
            (this Func<Task<T>> function, params TimeSpan[] intervals)
        {
            return intervals.Length == 0
                ? function()
                : Otherwise(
                    function(),
                    async () =>
                    {
                        await Task.Delay(intervals.First().Milliseconds);
                        return await Retry(function, intervals.Skip(1).ToArray()).ConfigureAwait(false);
                    });
        }
    }
}

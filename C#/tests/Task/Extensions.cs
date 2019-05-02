using System;
using System.Linq;
using System.Threading.Tasks;
using Radix.Tests;

namespace Task
{
    public static partial class Extensions
    {
        public static Task<T> AsTask<T>(this T t)
            => System.Threading.Tasks.Task.FromResult(t);

        public static async Task<TResult> Map<T, TResult>
            (this Task<T> task, Func<T, TResult> f)
            => f(await task);

        public static async Task<TResult> Select<T, TResult>
            (this Task<T> task, Func<T, TResult> f) => f(await task);

        public static Task<TResult> Map<T, TResult>
            (this Task<T> task, Func<AggregateException, TResult> faulted, Func<T, TResult> completed)
            => task.ContinueWith(
                t =>
                    t.Status == TaskStatus.Faulted
                        ? faulted(t.Exception)
                        : completed(t.Result));

        public static async Task<TResult> Bind<T, TResult>
            (this Task<T> task, Func<T, Task<TResult>> f)
            => await f(await task);

        public static async Task<TResult> SelectMany<T, TResult>
            (this Task<T> task, Func<T, Task<TResult>> f)
            => await f(await task);

        public static async Task<TResult> SelectMany<T, TResult>
            (this System.Threading.Tasks.Task task, Func<Unit, Task<T>> bind, Func<Unit, T, TResult> project)
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
            => task.ContinueWith(
                    t =>
                        t.Status == TaskStatus.Faulted
                            ? fallback()
                            : t.Result.AsTask()
                )
                .Unwrap();

        public static System.Threading.Tasks.Task Otherwise
            (this System.Threading.Tasks.Task task, Func<System.Threading.Tasks.Task> fallback)
            => task.ContinueWith(
                    t =>
                        t.Status == TaskStatus.Faulted
                            ? fallback()
                            : t
                )
                .Unwrap();

        

        public static async Task<Unit> ReturnUnit(this System.Threading.Tasks.Task t)
        {
            await t;
            return Unit.Instance;
        }


        /// <summary>
        ///     Retry with delays as long as the task is in a faulted state. 
        ///     The number of delays also indicate the number of retries
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="function"></param>
        /// <param name="delaysBetweenRetries">Delays between retries</param>
        /// <returns></returns>
        public static Task<T> Retry<T>
            (Func<Task<T>> function, params TimeSpan[] delaysBetweenRetries)
            => delaysBetweenRetries.Length == 0
                ? function()
                : function().Otherwise(
                    () =>
                        from _ in System.Threading.Tasks.Task.Delay(delaysBetweenRetries.First().Milliseconds)
                        from t in Retry(function, delaysBetweenRetries.Skip(1).ToArray())
                        select t);

        public static Task<T> Retry<T>
            (this Task<T> task, params TimeSpan[] delaysBetweenRetries)
            => Retry(() => task, delaysBetweenRetries);

        public static Task<Unit> Retry
            (this System.Threading.Tasks.Task task, params TimeSpan[] delaysBetweenRetries) =>
            Retry(task.ReturnUnit, delaysBetweenRetries);
    }
}

namespace Radix.Async;

public static class Extensions
{
    public static async Task<TResult> Select<T, TResult>
        (this Task<T> task, Func<T, TResult> f) => f(await task);

    public static async Task<TResult> Bind<T, TResult>
        (this Task<T> task, Func<T, Task<TResult>> f) => await f(await task);

    public static Task<TResult> SelectMany<T, TResult>
        (this Task<T> task, Func<T, Task<TResult>> f) => Bind(task, f);

    public static async Task<TResult> SelectMany<T, TResult>
        (this Task task, Func<Unit, Task<T>> bind, Func<Unit, T, TResult> project)
    {
        await task;
        T r = await bind(Unit.Instance);
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
        (this Task<T> task, Func<Task<T>> fallback) => task.ContinueWith(
        t => t.Status == TaskStatus.Faulted
            ? fallback()
            : Task.FromResult(t.Result)).Unwrap();

    /// <summary>
    ///     Filter tasks where the filter applies to its outcome.
    ///     Tasks of which the outcomes do not pass the filter are canceled
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <returns>The filtered task or the canceled task</returns>
    public static async Task<T> Where<T>(this Task<T> source
        , Func<T, bool> predicate)
    {
        T t = await source;
        if (!predicate(t))
        {
            return await Task.FromCanceled<T>(new CancellationToken(true));
        }

        return t;
    }

    /// <summary>
    ///     Only pass through tasks when the filter applies to any exception in the aggregate exception
    ///     Tasks of which the outcomes do not pass the filter are canceled
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="f"></param>
    /// <param name="exceptionFilter"></param>
    /// <returns></returns>
    public static Func<Task<T>> Where<T>(this Func<Task<T>> f
        , Func<Exception, bool> exceptionFilter) => () =>
        f().ContinueWith(
            t =>
            {
                switch (t.Status)
                {
                    case TaskStatus.Faulted:
                        {
                            bool? x = t.Exception?.InnerExceptions.Any(exceptionFilter);
                            if (x is not null && x == true)
                            {
                                return Task.FromResult(t.Result);
                            }

                            break;
                        }
                    case TaskStatus.Created:
                        break;
                    case TaskStatus.WaitingForActivation:
                        break;
                    case TaskStatus.WaitingToRun:
                        break;
                    case TaskStatus.Running:
                        break;
                    case TaskStatus.WaitingForChildrenToComplete:
                        break;
                    case TaskStatus.RanToCompletion:
                        break;
                    case TaskStatus.Canceled:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return Task.FromCanceled<T>(new CancellationToken(true));

            }).Unwrap();

    /// <summary>
    ///     Convenience overload for <see cref="Where{T}(System.Threading.Tasks.Task{T},System.Func{T,bool})" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="exceptionFilter"></param>
    /// <returns></returns>
    public static async Task<T> Where<T>(this Task<T> t
        , Func<Exception, bool> exceptionFilter) =>
        await Where(() => t, exceptionFilter)();


    /// <summary>
    ///     Retry and operation that is likely to succeed with delays as long as the task is in a faulted state.
    ///     The number of intervals also indicate the number of retries
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="function"></param>
    /// <param name="intervals">Delays between retries</param>
    /// <returns></returns>
    public static Task<T> Retry<T>
        (this Func<Task<T>> function, params TimeSpan[] intervals) => intervals.Length == 0
        ? function()
        : Otherwise(
            function(),
            async () =>
            {
                await Task.Delay(intervals.First().Milliseconds);
                return await Retry(function, intervals.Skip(1).ToArray()).ConfigureAwait(false);
            });

    /// <summary>
    ///     Convenience overload for <see cref="Retry{T}(System.Func{System.Threading.Tasks.Task{T}},System.TimeSpan[])" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="intervals"></param>
    /// <returns></returns>
    public static Task<T> Retry<T>
        (this Task<T> t, params TimeSpan[] intervals) =>
        Retry(() => t);
}

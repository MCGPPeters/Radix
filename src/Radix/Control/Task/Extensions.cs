using Radix.Data;

namespace Radix.Control.Task;

public static class Extensions
{
    public static async Task<Unit> Return(this System.Threading.Tasks.Task task)
    {
        await task;
        return Unit.Instance;
    }

    public static Task<T> Return<T>(T t) =>
        System.Threading.Tasks.Task.FromResult(t);

    /// <summary>
    /// Compose tasks to run sequentially.
    /// Choose this if there is if there is a dependency between tasks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="task"></param>
    /// <param name="f"></param>
    /// <returns></returns>
    public static async Task<TResult> Bind<T, TResult>
        (this Task<T> task, Func<T, Task<TResult>> f) =>
            await f(await task.ConfigureAwait(false)).ConfigureAwait(false);

    public static Task<TResult> SelectMany<T, TResult>
        (this Task<T> task, Func<T, Task<TResult>> f) => task.Bind(f);

    public static async Task<TResult> SelectMany<T, TResult>
        (this System.Threading.Tasks.Task task, Func<Unit, Task<T>> bind, Func<Unit, T, TResult> project)
    {
        await task;
        T r = await bind(Unit.Instance);
        return project(Unit.Instance, r);
    }

    public static async Task<TProjected> SelectMany<T, TResult, TProjected>
         (this Task<T> task, Func<T, Task<TResult>> bind, Func<T, TResult, TProjected> project)
    {
        T t = await task;
        TResult r = await bind(t);
        return project(t, r);
    }

    public static async Task<TResult> Map<T, TResult>
        (this Task<T> task, Func<T, TResult> f)
        => f(await task.ConfigureAwait(false));

    public static async Task<R> Map<R>
       (this System.Threading.Tasks.Task task, Func<R> f)
    {
        await task;
        return f();
    }

    public static Task<TResult> Map2<T, TResult>
     (this Task<T> task, Func<Exception, TResult> mapFaulted, Func<T, TResult> mapCompleted)
     => task.ContinueWith(t =>
           t.Status == TaskStatus.Faulted
              ? mapFaulted(t.Exception!)
              : mapCompleted(t.Result));

    public static Task<Func<T2, R>> Map<T1, T2, R>
       (this Task<T1> @this, Func<T1, T2, R> func)
        => @this.Map(func.Curry());

    public static Task<Func<T2, T3, R>> Map<T1, T2, T3, R>
       (this Task<T1> @this, Func<T1, T2, T3, R> func)
        => @this.Map(func.CurryFirst());

    public static Task<Func<T2, T3, T4, R>> Map<T1, T2, T3, T4, R>
       (this Task<T1> @this, Func<T1, T2, T3, T4, R> func)
        => @this.Map(func.CurryFirst());

    public static Task<Func<T2, T3, T4, T5, R>> Map<T1, T2, T3, T4, T5, R>
       (this Task<T1> @this, Func<T1, T2, T3, T4, T5, R> func)
        => @this.Map(func.CurryFirst());

    public static Task<Func<T2, T3, T4, T5, T6, R>> Map<T1, T2, T3, T4, T5, T6, R>
       (this Task<T1> @this, Func<T1, T2, T3, T4, T5, T6, R> func)
        => @this.Map(func.CurryFirst());

    public static Task<Func<T2, T3, T4, T5, T6, T7, R>> Map<T1, T2, T3, T4, T5, T6, T7, R>
       (this Task<T1> @this, Func<T1, T2, T3, T4, T5, T6, T7, R> func)
        => @this.Map(func.CurryFirst());

    public static Task<Func<T2, T3, T4, T5, T6, T7, T8, R>> Map<T1, T2, T3, T4, T5, T6, T7, T8, R>
       (this Task<T1> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func)
        => @this.Map(func.CurryFirst());

    public static Task<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Map<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
       (this Task<T1> @this, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func)
        => @this.Map(func.CurryFirst());

    public static async Task<TResult> Select<T, TResult>
        (this Task<T> task, Func<T, TResult> f) => await task.Map(f);


    /// <summary>
    /// Compose tasks to run in parallel
    /// Use this when tasks have no dependencies
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="f"></param>
    /// <param name="arg"></param>
    /// <returns></returns>
    public static async Task<TResult> Apply<T, TResult>
         (this Task<Func<T, TResult>> f, Task<T> arg)
         => (await f.ConfigureAwait(false))(await arg.ConfigureAwait(false));


    public static Task<Func<T2, R>> Apply<T1, T2, R>
         (this Task<Func<T1, T2, R>> f, Task<T1> arg)
         => f.Map(_.Curry).Apply(arg);

    public static Task<Func<T2, T3, R>> Apply<T1, T2, T3, R>
       (this Task<Func<T1, T2, T3, R>> @this, Task<T1> arg)
       => @this.Map(_.CurryFirst).Apply(arg);

    public static Task<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
       (this Task<Func<T1, T2, T3, T4, R>> @this, Task<T1> arg)
       => @this.Map(_.CurryFirst).Apply(arg);

    public static Task<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
       (this Task<Func<T1, T2, T3, T4, T5, R>> @this, Task<T1> arg)
       => @this.Map(_.CurryFirst).Apply(arg);

    public static Task<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
       (this Task<Func<T1, T2, T3, T4, T5, T6, R>> @this, Task<T1> arg)
       => @this.Map(_.CurryFirst).Apply(arg);

    public static Task<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
       (this Task<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Task<T1> arg)
       => @this.Map(_.CurryFirst).Apply(arg);

    public static Task<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
       (this Task<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Task<T1> arg)
       => @this.Map(_.CurryFirst).Apply(arg);

    public static Task<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
       (this Task<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Task<T1> arg)
       => @this.Map(_.CurryFirst).Apply(arg);


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
            : System.Threading.Tasks.Task.FromResult(t.Result)).Unwrap();

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
            return await System.Threading.Tasks.Task.FromCanceled<T>(new CancellationToken(true));
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
    public static Func<Task<T>> Catch<T>(this Func<Task<T>> f
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
                                return System.Threading.Tasks.Task.FromResult(t.Result);
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

                return System.Threading.Tasks.Task.FromCanceled<T>(new CancellationToken(true));

            }).Unwrap();

    /// <summary>
    ///     Convenience overload for <see cref="Where{T}(Task{T},Func{T,bool})" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="exceptionFilter"></param>
    /// <returns></returns>
    public static async Task<T> Catch<T>(this Task<T> t
        , Func<Exception, bool> exceptionFilter) =>
        await Catch(() => t, exceptionFilter)();


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
                await System.Threading.Tasks.Task.Delay(intervals.First().Milliseconds);
                return await Retry(function, intervals.Skip(1).ToArray()).ConfigureAwait(false);
            });

    /// <summary>
    ///     Convenience overload for <see cref="Retry{T}(Func{Task{T}},TimeSpan[])" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="intervals"></param>
    /// <returns></returns>
    public static Task<T> Retry<T>
        (this Task<T> t, params TimeSpan[] intervals) =>
        Retry(() => t);
}

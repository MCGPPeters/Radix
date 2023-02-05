using static Radix.Control.Result.Extensions;
using Radix.Control.Result;
using Radix.Data;

namespace Radix.Control.Task.Result;

public class TaskResult<T, TError> : Task<Result<T, TError>>
{
    public TaskResult(Func<object?, Result<T, TError>> function, object? state) : base(function, state)
    {
    }

    public TaskResult(Func<object?, Result<T, TError>> function, object? state, CancellationToken cancellationToken) : base(function, state, cancellationToken)
    {
    }

    public TaskResult(Func<object?, Result<T, TError>> function, object? state, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, state, cancellationToken, creationOptions)
    {
    }

    public TaskResult(Func<object?, Result<T, TError>> function, object? state, TaskCreationOptions creationOptions) : base(function, state, creationOptions)
    {
    }

    public TaskResult(Func<Result<T, TError>> function) : base(function)
    {
    }

    public TaskResult(Func<Result<T, TError>> function, CancellationToken cancellationToken) : base(function, cancellationToken)
    {
    }

    public TaskResult(Func<Result<T, TError>> function, CancellationToken cancellationToken, TaskCreationOptions creationOptions) : base(function, cancellationToken, creationOptions)
    {
    }

    public TaskResult(Func<Result<T, TError>> function, TaskCreationOptions creationOptions) : base(function, creationOptions)
    {
    }
}

public static class Extensions
{

    public static Task<Result<T, TError>> Return<T, TError>(this T result)
        where T : notnull =>
        System.Threading.Tasks.Task.FromResult(Ok<T, TError>(result));

    public static TaskResult<T, TError> ReturnError<T, TError>(this TError error)
        where T : notnull =>
        (TaskResult<T, TError>)System.Threading.Tasks.Task.FromResult(Error<T, TError>(error));

    public static TaskResult<TResult, TError> Select<T, TResult, TError>
       (this TaskResult<T, TError> result
       , Func<T, TResult> mapper)
        where T : notnull
        where TResult : notnull
       => (TaskResult<TResult, TError>)result.Map(x => x.Map(mapper));

    public static TaskResult<TResult, TError> Traverse<T, TResult, TError>
      (this TaskResult<T, TError> task, Func<T, TaskResult<TResult, TError>> f)
        where T : notnull
      => task.GetAwaiter().GetResult() switch
      {
          Error<T, TError>(var reasons) => (TaskResult<TResult, TError>)System.Threading.Tasks.Task.FromResult(Error<TResult, TError>(reasons)),
          Ok<T, TError>(var ok) => f(ok),
          _ => throw new NotImplementedException()
      };

    public static TaskResult<TResult, TError> TraverseBind<T, TResult, TError>(this Result<T, TError> result
       , Func<T, TaskResult<TResult, TError>> f)
        where T : notnull
       => result switch
       {
           Error<T, TError>(var reasons) => (TaskResult<TResult, TError>)System.Threading.Tasks.Task.FromResult(Error<TResult, TError>(reasons)),
           Ok<T, TError>(var ok) => f(ok),
           _ => throw new NotImplementedException()
       };

    public static TaskResult<TResult, TError> SelectMany<T, TResult, TError>
       (this TaskResult<T, TError> task
       , Func<T, TaskResult<TResult, TError>> bind)
        where T : notnull
       => (TaskResult<TResult, TError>)task.Bind(vt => vt.TraverseBind(bind));

    public static TaskResult<TProjected, TError> SelectMany<T, TResult, TProjected, TError>
    (this TaskResult<T, TError> task
        , Func<T, TaskResult<TResult, TError>> bind
        , Func<T, TResult, TProjected> project)
        where T : notnull
        where TResult : notnull
        where TProjected : notnull
        => task
            .Select(vt => vt.TraverseBind(t =>
                (TaskResult<TProjected, TError>)bind(t).Select(vr => vr.Select(r => project(t, r))))).GetAwaiter()
            .GetResult();

    public  static async Task<Result<TResult, TError>> Apply<T, TResult, TError>
         (this Task<Func<Result<T, TError>, Result<TResult, TError>>> f, Task<Result<T, TError>> task)
         => (await f.ConfigureAwait(false))(await task.ConfigureAwait(false));


    public static Task<Func<Result<T2, TError>, Result<R, TError>>> Apply<T1, T2, R, TError>
         (this Task<Func<Result<T1, TError>, Result<T2, TError>, Result<R, TError>>> f, Task<Result<T1, TError>> arg)
         => f.Map(Prelude.Curry).Apply(arg);

    public static Task<Func<Result<T2, TError>, Result<T3, TError>, Result<R, TError>>> Apply<T1, T2, T3, R, TError>
       (this Task<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<R, TError>>> @this, Task<Result<T1, TError>> arg)
       => @this.Map(Prelude.CurryFirst).Apply(arg);

    public static Task<Func<Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<R, TError>>> Apply<T1, T2, T3, T4, R, TError>
       (this Task<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<R, TError>>> @this, Task<Result<T1, TError>> arg)
       => @this.Map(Prelude.CurryFirst).Apply(arg);

    public static Task<Func<Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<R, TError>>> Apply<T1, T2, T3, T4, T5, R, TError>
       (this Task<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<R, TError>>> @this, Task<Result<T1, TError>> arg)
       => @this.Map(Prelude.CurryFirst).Apply(arg);

    public static Task<Func<Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<T6, TError>, Result<R, TError>>> Apply<T1, T2, T3, T4, T5, T6, R, TError>
       (this Task<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<T6, TError>, Result<R, TError>>> @this, Task<Result<T1, TError>> arg)
       => @this.Map(Prelude.CurryFirst).Apply(arg);

    public static Task<Func<Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<T6, TError>, Result<T7, TError>, Result<R, TError>>> Apply<T1, T2, T3, T4, T5, T6, T7, R, TError>
       (this Task<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<T6, TError>, Result<T7, TError>, Result<R, TError>>> @this, Task<Result<T1, TError>> arg)
       => @this.Map(Prelude.CurryFirst).Apply(arg);

    public static Task<Func<Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<T6, TError>, Result<T7, TError>, Result<T8, TError>, Result<R, TError>>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R, TError>
       (this Task<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<T6, TError>, Result<T7, TError>, Result<T8, TError>, Result<R, TError>>> @this, Task<Result<T1, TError>> arg)
       => @this.Map(Prelude.CurryFirst).Apply(arg);

    public static Task<Func<Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<T6, TError>, Result<T7, TError>, Result<T8, TError>, Result<T9, TError>, Result<R, TError>>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R, TError>
       (this Task<Func<Result<T1, TError>, Result<T2, TError>, Result<T3, TError>, Result<T4, TError>, Result<T5, TError>, Result<T6, TError>, Result<T7, TError>, Result<T8, TError>, Result<T9, TError>, Result<R, TError>>> @this, Task<Result<T1, TError>> arg)
       => @this.Map(Prelude.CurryFirst).Apply(arg);
}


using static Radix.Control.Result.Extensions;
using Radix.Control.Result;
using Radix.Data;

namespace Radix.Control.Task.Result;

public static class Extensions
{

    public static Task<Result<T, TError>> Return<T, TError>(T result)
        where T : notnull =>
        System.Threading.Tasks.Task.FromResult(Ok<T, TError>(result));

    public static Task<Result<TResult, TError>> Select<T, TResult, TError>
       (this Task<Result<T, TError>> result
       , Func<T, TResult> mapper)
        where T : notnull
        where TResult : notnull
       => result.Map(x => x.Map(mapper));

    public static Task<Result<TResult, TError>> Traverse<T, TResult, TError>
      (this Task<Result<T, TError>> task, Func<T, Task<TResult>> f)
        where T : notnull
      => task switch
      {
          Error<T, TError>(var reasons) => System.Threading.Tasks.Task.FromResult(Error<TResult, TError>(reasons)),
          Ok<T, TError>(var ok) => f(ok).Map(Ok<TResult, TError>),
          _ => throw new NotImplementedException()
      };

    public static Task<Result<TResult, TError>> TraverseBind<T, TResult, TError>(this Result<T, TError> result
       , Func<T, Task<Result<TResult, TError>>> f)
        where T : notnull
       => result switch
       {
           Error<T, TError>(var reasons) => System.Threading.Tasks.Task.FromResult(Error<TResult, TError>(reasons)),
           Ok<T, TError>(var ok) => f(ok),
           _ => throw new NotImplementedException()
       };

    public static Task<Result<TResult, TError>> SelectMany<T, TResult, TError>
       (this Task<Result<T, TError>> task
       , Func<T, Task<Result<TResult, TError>>> bind)
        where T : notnull
       => task.Bind(vt => vt.TraverseBind(bind));

    public static Task<Result<TProjected, TError>> SelectMany<T, TResult, TProjected, TError>
       (this Task<Result<T, TError>> task
       , Func<T, Task<Result<TResult, TError>>> bind
       , Func<T, TResult, TProjected> project)
        where T : notnull
        where TResult : notnull
        where TProjected : notnull
       => task
          .Map(vt => vt.TraverseBind(t => bind(t).Map(vr => vr.Map(r => project(t, r)))))
          .Unwrap();

    public async static Task<Result<TResult, TError>> Apply<T, TResult, TError>
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


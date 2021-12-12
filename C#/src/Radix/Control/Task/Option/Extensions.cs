using static Radix.Control.Option.Extensions;
using Radix.Control.Result;
using Radix.Data;

namespace Radix.Control.Task.Option;

public static class Extensions
{
    public static Task<Option<TResult>> Select<T, TResult>
       (this Task<Option<T>> task
       , Func<T, TResult> mapper)
       => task.Map(x => x.Map(mapper));

    public static Task<Option<TResult>> Traverse<T, TResult>
      (this Option<T> task, Func<T, Task<TResult>> f)
      => task switch
      {
          None<T> => System.Threading.Tasks.Task.FromResult(None<TResult>()),
          Some<T>(var some) => f(some).Map(Some),
          _ => throw new NotImplementedException()
      };

    public static Task<Option<TResult>> TraverseBind<T, TResult>(this Option<T> task
       , Func<T, Task<Option<TResult>>> f)
       => task switch
       {
           None<T> => System.Threading.Tasks.Task.FromResult(None<TResult>()),
           Some<T>(var some) => f(some),
           _ => throw new NotImplementedException()
       };

    public static Task<Option<TResult>> SelectMany<T, TResult>
       (this Task<Option<T>> task
       , Func<T, Task<Option<TResult>>> bind)
       => task.Bind(vt => vt.TraverseBind(bind));

    public static Task<Option<RR>> SelectMany<T, R, RR>
       (this Task<Option<T>> task
       , Func<T, Task<Option<R>>> bind
       , Func<T, R, RR> project)
       => task
          .Map(vt => vt.TraverseBind(t => bind(t).Map(vr => vr.Map(r => project(t, r)))))
          .Unwrap();
}


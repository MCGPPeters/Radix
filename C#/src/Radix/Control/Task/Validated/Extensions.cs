using static Radix.Control.Validated.Extensions;
using Radix.Control.Result;
using Radix.Data;

namespace Radix.Control.Task.Validated;

public static class Extensions
{
    public static Task<Validated<TResult>> Select<T, TResult>
       (this Task<Validated<T>> task
       , Func<T, TResult> mapper)
       => task.Map(x => x.Map(mapper));

    public static Task<Validated<TResult>> Traverse<T, TResult>
      (this Validated<T> task, Func<T, Task<TResult>> f)
      => task switch
      {
          Invalid<T>(var reasons) => System.Threading.Tasks.Task.FromResult(Invalid<TResult>(reasons)),
          Valid<T>(var valid) => f(valid).Map(Valid),
          _ => throw new NotImplementedException()
      };

    public static Task<Validated<TResult>> TraverseBind<T, TResult>(this Validated<T> task
       , Func<T, Task<Validated<TResult>>> f)
       => task switch
       {
           Invalid<T>(var reasons) => System.Threading.Tasks.Task.FromResult(Invalid<TResult>(reasons)),
           Valid<T>(var valid) => f(valid),
           _ => throw new NotImplementedException()
       };

    public static Task<Validated<TResult>> SelectMany<T, TResult>
       (this Task<Validated<T>> task
       , Func<T, Task<Validated<TResult>>> bind)
       => task.Bind(vt => vt.TraverseBind(bind));

    public static Task<Validated<RR>> SelectMany<T, R, RR>
       (this Task<Validated<T>> task
       , Func<T, Task<Validated<R>>> bind
       , Func<T, R, RR> project)
       => task
          .Map(vt => vt.TraverseBind(t => bind(t).Map(vr => vr.Map(r => project(t, r)))))
          .Unwrap();
}


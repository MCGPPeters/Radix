using Radix.Control.Option;
using Radix.Control.Task;
using Radix.Data;

using static Radix.Control.Option.Extensions;

namespace Radix.Control.Option;
public static class Extensions
{
    public static Option<T> Some<T>(T value) where T : notnull =>
        new Some<T>(value);

    public static Option<T> None<T>() => Data.None<T>.Default;

    public static Option<TResult> Bind<T, TResult>
        (this Option<T> option, Func<T, Option<TResult>> f)
        where T : notnull
        where TResult : notnull => option switch
        {
            Some<T>(var t) => f(t),
            None<T> _ => None<TResult>(),
            _ => throw new NotSupportedException()
        };

    public static Task<Option<TResult>> Traverse<T, TResult>
         (this Option<T> @this, Func<T, Task<TResult>> f)
         =>
            @this switch
            {
                Some<T>(var t) => f(t).Map(Some),
                None<T> _ => System.Threading.Tasks.Task.FromResult(None<TResult>()),
                _ => throw new NotSupportedException()
            };

    public static Option<TResult> Map<T, TResult>
        (this Option<T> option, Func<T, TResult> f)
        where T : notnull
        where TResult : notnull => option switch
        {
            Some<T>(var t) => Some(f(t)),
            None<T> _ => None<TResult>(),
            _ => throw new NotSupportedException()
        };

    public static Option<TResult> Select<T, TResult>
         (this Option<T> option, Func<T, TResult> f)
        where T : notnull
        where TResult : notnull =>
            option.Map(f);

    public static Option<TResult> SelectMany<T, TIntermediate, TResult>
     (this Option<T> option, Func<T, Option<TIntermediate>> bind, Func<T, TIntermediate, TResult> project)
        where T : notnull
        where TResult : notnull
     => option switch
     {
         None<T> => None<TResult>(),
         Some<T>(var t) =>
          bind(t) switch
          {
              None<TIntermediate> => None<TResult>(),
              Some<TIntermediate>(var x) => Some(project(t, x)),
              _ => throw new NotImplementedException()
          },
         _ => throw new NotImplementedException()
     };
}

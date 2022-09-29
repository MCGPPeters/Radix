using Radix.Data;

namespace Radix.Control.Result;

public static class Extensions
{
    public static Result<T, TError> Ok<T, TError>(T t) where T : notnull => new Ok<T, TError>(t);

    public static Result<T, TError> Error<T, TError>(TError error) => new Error<T, TError>(error);

    public static Result<TResult, TError> Bind<T, TResult, TError>(this Result<T, TError> result, Func<T, Result<TResult, TError>> function) where T : notnull => result switch
    {
        Ok<T, TError>(var value) => function(value),
        Error<T, TError>(var error) => Error<TResult, TError>(error),
        _ => throw new NotSupportedException("Unlikely")
    };

    public static Result<TResult, TError> SelectMany<T, TResult, TError>(this Result<T, TError> result, Func<T, Result<TResult, TError>> function) where T : notnull
        => result.Bind(function);

    public static Result<TResult, TError> SelectMany<T, TIntermediate, TResult, TError>(
        this Result<T, TError> result,
        Func<T, Result<TIntermediate, TError>> bind, Func<T, TIntermediate, TResult> project)
        where T : notnull
        where TIntermediate : notnull
        where TResult : notnull
        => result switch
        {
            Ok<T, TError>(var t) =>
                bind(t) switch
                {
                    Ok<TIntermediate, TError> x => Ok<TResult, TError>(project(t, x)),
                    Error<TIntermediate, TError>(var error) => Error<TResult, TError>(error),
                    _ => throw new NotSupportedException("Unlikely")
                },
            Error<T, TError>(var error) => Error<TResult, TError>(error),
            _ => throw new NotSupportedException("Unlikely")
        };

    public static Result<TResult, TError> Map<T, TResult, TError>(this Result<T, TError> result, Func<T, TResult> function)
        where TResult : notnull
        where T : notnull
        => result switch
    {
        Ok<T, TError>(var value) => Ok<TResult, TError>(function(value)),
        Error<T, TError>(var error) => Error<TResult, TError>(error),
        _ => throw new NotSupportedException("Unlikely")
    };

    public static Result<TResult, TError> Select<T, TResult, TError>(this Result<T, TError> result, Func<T, TResult> function)
        where TResult : notnull
        where T : notnull
        => result.Map(function);


    public static Result<T, TErrorResult> MapError<T, TError, TErrorResult>(this Result<T, TError> result, Func<TError, TErrorResult> function)
        where T : notnull => result switch
    {
        Ok<T, TError>(var value) => Ok<T, TErrorResult>(value),
        Error<T, TError>(var error) => Error<T, TErrorResult>(function(error)),
        _ => throw new NotSupportedException("Unlikely")
    };

    public static Result<TResult, TError> Apply<T, TResult, TError>(this Result<Func<T, TResult>, TError> fResult, Result<T, TError> xResult)
        where T : notnull
        where TResult : notnull => (fResult, xResult) switch
    {
        (Ok<Func<T, TResult>, TError>(var f), Ok<T, TError>(var x)) => Ok<TResult, TError>(f(x)),
        (Error<Func<T, TResult>, TError>(var error), Ok<T, TError>(_)) => Error<TResult, TError>(error),
        (Ok<Func<T, TResult>, TError>(_), Error<T, TError>(var error)) => Error<TResult, TError>(error),
        (Error<Func<T, TResult>, TError>(var error), Error<T, TError> _) => Error<TResult, TError>(error),
        _ => throw new NotSupportedException("Unlikely")
    };

    public static Result<Func<T2, R>, TError> Apply<TError, T1, T2, R>
       (this Result<Func<T1, T2, R>, TError> @this, Result<T1, TError> arg)
        where T1 : notnull
       => Apply(@this.Map(Prelude.Curry), arg);

    public static Result<Func<T2, T3, R>, TError> Apply<TError, T1, T2, T3, R>
       (this Result<Func<T1, T2, T3, R>, TError> @this, Result<T1, TError> arg)
        where T1 : notnull
       => Apply(@this.Map(Prelude.CurryFirst), arg);

    public static Result<Func<T2, T3, T4, R>, TError> Apply<TError, T1, T2, T3, T4, R>
       (this Result<Func<T1, T2, T3, T4, R>, TError> @this, Result<T1, TError> arg)
        where T1 : notnull
       => Apply(@this.Map(Prelude.CurryFirst), arg);

    public static Result<Func<T2, T3, T4, T5, R>, TError> Apply<TError, T1, T2, T3, T4, T5, R>
       (this Result<Func<T1, T2, T3, T4, T5, R>, TError> @this, Result<T1, TError> arg)
        where T1 : notnull
       => Apply(@this.Map(Prelude.CurryFirst), arg);

    public static Result<Func<T2, T3, T4, T5, T6, R>, TError> Apply<TError, T1, T2, T3, T4, T5, T6, R>
       (this Result<Func<T1, T2, T3, T4, T5, T6, R>, TError> @this, Result<T1, TError> arg)
        where T1 : notnull
       => Apply(@this.Map(Prelude.CurryFirst), arg);

    public static Result<Func<T2, T3, T4, T5, T6, T7, R>, TError> Apply<TError, T1, T2, T3, T4, T5, T6, T7, R>
       (this Result<Func<T1, T2, T3, T4, T5, T6, T7, R>, TError> @this, Result<T1, TError> arg)
        where T1 : notnull
       => Apply(@this.Map(Prelude.CurryFirst), arg);

    public static Result<Func<T2, T3, T4, T5, T6, T7, T8, R>, TError> Apply<TError, T1, T2, T3, T4, T5, T6, T7, T8, R>
       (this Result<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>, TError> @this, Result<T1, TError> arg)
        where T1 : notnull
       => Apply(@this.Map(Prelude.CurryFirst), arg);

    public static Result<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>, TError> Apply<TError, T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
       (this Result<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>, TError> @this, Result<T1, TError> arg)
        where T1 : notnull
       => Apply(@this.Map(Prelude.CurryFirst), arg);
}

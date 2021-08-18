namespace Radix.Result;

public static class Extensions
{
    public static Result<T, TError> Ok<T, TError>(T t) where T : notnull => new Ok<T, TError>(t);

    public static Result<T, TError> Error<T, TError>(TError error) => new Error<T, TError>(error);

    public static Result<TResult, TError> Bind<T, TResult, TError>(this Result<T, TError> result, Func<T, Result<TResult, TError>> function) where TResult : notnull where T : notnull => result switch
    {
        Ok<T, TError>(var value) => function(value),
        Error<T, TError>(var error) => Error<TResult, TError>(error),
        _ => throw new NotSupportedException("Unlikely")
    };

    public static Result<TResult, TError> SelectMany<T, TResult, TError>(this Result<T, TError> result, Func<T, Result<TResult, TError>> function) where TResult : notnull where T : notnull
        => Bind(result, function);

    public static Result<TResult, TError> SelectMany<T, TIntermediate, TResult, TError>(
        this Result<T, TError> result,
        Func<T, Result<TIntermediate, TError>> bind, Func<T, TIntermediate, TResult> project) where TResult : notnull where T : notnull where TIntermediate : notnull
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

    public static Result<TResult, TError> Map<T, TResult, TError>(this Result<T, TError> result, Func<T, TResult> function) where TResult : notnull where T : notnull => result switch
    {
        Ok<T, TError>(var value) => Ok<TResult, TError>(function(value)),
        Error<T, TError>(var error) => Error<TResult, TError>(error),
        _ => throw new NotSupportedException("Unlikely")
    };

    public static Result<TResult, TError> Select<T, TResult, TError>(this Result<T, TError> result, Func<T, TResult> function) where TResult : notnull where T : notnull => Map(result, function);


    public static Result<T, TErrorResult> MapError<T, TError, TErrorResult>(this Result<T, TError> result, Func<TError, TErrorResult> function) where T : notnull => result switch
    {
        Ok<T, TError>(var value) => Ok<T, TErrorResult>(value),
        Error<T, TError>(var error) => Error<T, TErrorResult>(function(error)),
        _ => throw new NotSupportedException("Unlikely")
    };

    public static Result<TResult, TError> Apply<T, TResult, TError>(this Result<Func<T, TResult>, TError> fResult, Result<T, TError> xResult) where TResult : notnull where T : notnull => (fResult, xResult) switch
    {
        (Ok<Func<T, TResult>, TError>(var f), Ok<T, TError>(var x)) => Ok<TResult, TError>(f(x)),
        (Error<Func<T, TResult>, TError>(var error), Ok<T, TError>(_)) => Error<TResult, TError>(error),
        (Ok<Func<T, TResult>, TError>(_), Error<T, TError>(var error)) => Error<TResult, TError>(error),
        (Error<Func<T, TResult>, TError>(var error), Error<T, TError> _) => Error<TResult, TError>(error),
        _ => throw new NotSupportedException("Unlikely")
    };
}

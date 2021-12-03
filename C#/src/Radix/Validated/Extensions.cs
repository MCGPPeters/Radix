namespace Radix.Validated;

public static class Extensions
{
    public static Validated<T> Valid<T>(T t) => new Valid<T>(t);

    public static Validated<T> Invalid<T>(params string[] reasons) => new Invalid<T>(reasons);

    public static Validated<TResult> Bind<T, TResult>(this Validated<T> result, Func<T, Validated<TResult>> function) => result switch
    {
        Valid<T>(var valid) => function(valid),
        Invalid<T>(var reasons) => Invalid<TResult>(reasons),
        _ => throw new NotSupportedException("Unlikely")
    };

    /// <summary>
    ///     For linq syntax support
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="result"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    public static Validated<TResult> SelectMany<T, TResult>(this Validated<T> result, Func<T, Validated<TResult>> function) => result.Bind(function);

    public static Validated<TProjection> SelectMany<T, TResult, TProjection>(this Validated<T> result, Func<T, Validated<TResult>> function,
        Func<T, TResult, TProjection> project) => result switch
        {
            Valid<T>(var valid) => function(valid).Bind(r => Valid(project(valid, r))),
            Invalid<T>(var reasons) => Invalid<TProjection>(reasons),
            _ => throw new NotSupportedException("Unlikely")
        };

    public static Validated<TResult> Map<T, TResult>(this Validated<T> result, Func<T, TResult> function) => result.Bind(x => Valid(function(x)));

    /// <summary>
    ///     For linq syntax support
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="result"></param>
    /// <param name="function"></param>
    /// <returns></returns>
    public static Validated<TResult> Select<T, TResult>(this Validated<T> result, Func<T, TResult> function) => result.Map(function);

    public static Validated<TResult> Apply<T, TResult>(this Validated<Func<T, TResult>> fValidated, Validated<T> xValidated) =>
        (fValidated, xValidated) switch
        {
            (Valid<Func<T, TResult>>(var f), Valid<T>(var x)) => Valid(f(x)),
            (Invalid<Func<T, TResult>>(var error), Valid<T>(_)) => Invalid<TResult>(error),
            (Valid<Func<T, TResult>>(_), Invalid<T>(var error)) => Invalid<TResult>(error),
            (Invalid<Func<T, TResult>>(var error), Invalid<T>(var otherError)) => Invalid<TResult>(error.Concat(otherError).ToArray()),
            _ => throw new NotSupportedException("Unlikely")
        };

    public static IEnumerable<T> WhereValid<T>(this IEnumerable<Validated<T>> xs)
    {
        foreach (Validated<T> validated in xs)
        {
            switch (validated)
            {
                case Valid<T>(var valid):
                    yield return valid;
                    break;
                default:
                    continue;
            }

            ;
        }


    }

    public static IEnumerable<string[]> WhereNotValid<T>(this IEnumerable<Validated<T>> xs)
    {
        foreach (Validated<T> validated in xs)
        {
            switch (validated)
            {
                case Invalid<T>(var errorMessages):
                    yield return errorMessages;
                    break;
                default:
                    continue;
            }

            ;
        }
    }

    public static Validated<Func<T2, R>> Apply<T1, T2, R>
        (this Validated<Func<T1, T2, R>> @this, Validated<T1> arg) => Apply(@this.Map(_.Curry), arg);

    public static Validated<Func<T2, T3, R>> Apply<T1, T2, T3, R>
        (this Validated<Func<T1, T2, T3, R>> @this, Validated<T1> arg) => Apply(@this.Map(_.CurryFirst), arg);

    public static Validated<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
        (this Validated<Func<T1, T2, T3, T4, R>> @this, Validated<T1> arg) => Apply(@this.Map(_.CurryFirst), arg);

    public static Validated<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
        (this Validated<Func<T1, T2, T3, T4, T5, R>> @this, Validated<T1> arg) => Apply(@this.Map(_.CurryFirst), arg);

    public static Validated<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
        (this Validated<Func<T1, T2, T3, T4, T5, T6, R>> @this, Validated<T1> arg) => Apply(@this.Map(_.CurryFirst), arg);

    public static Validated<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
        (this Validated<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Validated<T1> arg) => Apply(@this.Map(_.CurryFirst), arg);

    public static Validated<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
        (this Validated<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Validated<T1> arg) => Apply(@this.Map(_.CurryFirst), arg);

    public static Validated<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
        (this Validated<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Validated<T1> arg) => Apply(@this.Map(_.CurryFirst), arg);
}

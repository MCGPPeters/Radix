using System;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Tests.Validated
{
    public static class Extensions
    {
        public static Validated<T> Valid<T>(T t)
        {
            return new Valid<T>(t);
        }

        public static Validated<T> Invalid<T>(IEnumerable<string> reasons)
        {
            return new Invalid<T>(reasons);
        }

        public static Validated<TResult> Bind<T, TResult>(this Validated<T> result, Func<T, Validated<TResult>> function)
            => result switch
            {
                Valid<T>(var valid) => function(valid),
                Invalid<T>(var reasons) => Invalid<TResult>(reasons),
                _ => throw new NotSupportedException("Unlikely")
            };

        public static Validated<TResult> Map<T, TResult>(this Validated<T> result, Func<T, TResult> function)
            => result.Bind(x => Valid(function(x)));

        public static Validated<TResult> Apply<T, TResult>(this Validated<Func<T, TResult>> fValidated, Validated<T> xValidated)
        {
            return (fValidated, xValidated) switch
            {
                (Valid<Func<T, TResult>>(var f), Valid<T>(var x)) => Valid(f(x)),
                (Invalid<Func<T, TResult>>(var error), Valid<T>(_)) => Invalid<TResult>(error),
                (Valid<Func<T, TResult>>(_), Invalid<T>(var error)) => Invalid<TResult>(error),
                (Invalid<Func<T, TResult>>(var error), Invalid<T>(var otherError)) => Invalid<TResult>(error.Concat(otherError)),
                _ => throw new NotSupportedException("Unlikely")
            };
        }

        //public static Validated<T> Check<T>(this T subject, Func<T, bool> rule, Func<T, string> mapReason)
        //{
        //    Validated<T> validate(T arg) => rule(arg) ? Valid(arg) : Invalid<T>(new List<string> { mapReason(arg) });
        //    return Valid(_<T>.Id).Apply(validate(subject));
        //}


        //public static Validated<T> Check<T>(this Validated<T> validated, Func<T, bool> rule, Func<T, string> mapReason)
        //{
        //    Validated<T> validate(T arg) => rule(arg) ? Valid(arg) : Invalid<T>(new List<string> { mapReason(arg) });

        //    switch (validated)
        //    {
        //        case Valid<T>(var valid):
        //            return Valid(_ => valid).Apply(validate(valid));
        //        case Invalid<T>(var invalid):
        //            return Invalid(_ => invalid).Apply(validate(subject));
        //        default: throw new NotSupportedException("Unlikely");
        //    }
        //}


        public static Validated<Func<T2, R>> Apply<T1, T2, R>
            (this Validated<Func<T1, T2, R>> @this, Validated<T1> arg)
            => Apply(@this.Map(_.Curry), arg);

        public static Validated<Func<T2, T3, R>> Apply<T1, T2, T3, R>
            (this Validated<Func<T1, T2, T3, R>> @this, Validated<T1> arg)
            => Apply(@this.Map(_.CurryFirst), arg);

        public static Validated<Func<T2, T3, T4, R>> Apply<T1, T2, T3, T4, R>
            (this Validated<Func<T1, T2, T3, T4, R>> @this, Validated<T1> arg)
            => Apply(@this.Map(_.CurryFirst), arg);

        public static Validated<Func<T2, T3, T4, T5, R>> Apply<T1, T2, T3, T4, T5, R>
            (this Validated<Func<T1, T2, T3, T4, T5, R>> @this, Validated<T1> arg)
            => Apply(@this.Map(_.CurryFirst), arg);

        public static Validated<Func<T2, T3, T4, T5, T6, R>> Apply<T1, T2, T3, T4, T5, T6, R>
            (this Validated<Func<T1, T2, T3, T4, T5, T6, R>> @this, Validated<T1> arg)
            => Apply(@this.Map(_.CurryFirst), arg);

        public static Validated<Func<T2, T3, T4, T5, T6, T7, R>> Apply<T1, T2, T3, T4, T5, T6, T7, R>
            (this Validated<Func<T1, T2, T3, T4, T5, T6, T7, R>> @this, Validated<T1> arg)
            => Apply(@this.Map(_.CurryFirst), arg);

        public static Validated<Func<T2, T3, T4, T5, T6, T7, T8, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, R>
            (this Validated<Func<T1, T2, T3, T4, T5, T6, T7, T8, R>> @this, Validated<T1> arg)
            => Apply(@this.Map(_.CurryFirst), arg);

        public static Validated<Func<T2, T3, T4, T5, T6, T7, T8, T9, R>> Apply<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>
            (this Validated<Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>> @this, Validated<T1> arg)
            => Apply(@this.Map(_.CurryFirst), arg);

    }
}

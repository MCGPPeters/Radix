//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//using static Radix.Tests.Validated.Extensions;

//namespace Radix.Tests.Validated;

//public static class Extensions
//{
//    public static Validated<T> Valid<T>(T t) => new Valid<T>(t);

//    public static Validated<T> Invalid<T>(params string[] reasons) => new Invalid<T>(reasons);
//}

//public interface Validated { }

//public sealed record Valid<T>(T Value) : Validated<T>
//{
//    public static implicit operator Valid<T>(T t) => new(t);
//}

//public sealed record Invalid<T>(params string[] Reasons) : Validated<T>
//{
//    public static implicit operator Invalid<T>(string[] reasons) => new(reasons);

//    public static implicit operator string[](Invalid<T> invalid) => invalid.Reasons;
//}

//public record Validated<T> :
//    Data<Validated, T>,
//    Monad<Validated, T>
//{
//    public Data<Validated, R> Apply<R>(Data<Validated, Func<T, R>> f, Data<Validated, T> t) => throw new NotImplementedException();
//    public Data<Validated, R> Bind<R>(Func<T, Data<Validated, R>> f) =>
//        this switch
//        {
//            Valid<T>(var valid) => f(valid),
//            Invalid<T>(var reasons) => Invalid<T>(reasons),
//            _ => throw new NotSupportedException("Unlikely")
//        };

//    public Data<Validated, R> Map<R>(Func<T, R> f) => throw new NotImplementedException();
//    public Data<Validated, T> Pure(T t) => throw new NotImplementedException();
//}

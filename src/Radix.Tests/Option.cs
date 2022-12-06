//namespace Radix.Tests.Option;

//public interface Option { }

//public sealed record None<T> : Option<T> { }

//public sealed record Some<T>(T Value) : Option<T> { }

//public record Option<T> :
//    Data<Option, T>,
//    Functor<Option, T>,
//    Applicative<Option, T>,
//    Monad<Option, T>
//{
//    public Data<Option, R> Apply<R>(Data<Option, Func<T, R>> f, Data<Option, T> t) =>
//        f switch
//        {
//            None<Func<T, R>> _ => new None<R>(),
//            Some<Func<T, R>> (var g) => Map(g),
//            _ => throw new NotSupportedException()
//        };

//    public Data<Option, R> Bind<R>(Func<T, Data<Option, R>> f) =>
//        this switch
//        {
//            Some<T>(var t) => f(t),
//            None<T> _ => new None<R>(),
//            _ => throw new NotSupportedException()
//        };

//    public Data<Option, R> Map<R>(Func<T, R> f) =>
//        this switch
//        {
//            Some<T>(var t) => new Some<R>(f(t)),
//            None<T> _ => new None<R>(),
//            _ => throw new NotSupportedException()
//        };

//    public Data<Option, T> Pure(T t) => 
//        new Some<T>(t);
        

//}

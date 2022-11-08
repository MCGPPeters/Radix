using System.Runtime.CompilerServices;
using XPlot.Plotly;

namespace Radix.Tests;

public interface Data<Type, Kind>
{

}

public interface Option { }

public record Option<T> :
    Data<Option, T>,
    Functor<Option, T>,
    Applicative<Option, T>,
    Monad<Option, T>
{


    public Data<Option, R> Apply<R>(Data<Option, Func<T, R>> f, Data<Option, T> t) =>
        f switch
        {
            None<Func<T, R>> _ => new None<R>(),
            Some<Func<T, R>> (var g) => Map(g),
            _ => throw new NotSupportedException()
        };
    public Data<Option, R> Bind<R>(Func<T, Data<Option, R>> f) =>
        this switch
        {
            Some<T>(var t) => f(t),
            None<T> _ => new None<R>(),
            _ => throw new NotSupportedException()
        };

    public Data<Option, R> Map<R>(Func<T, R> f) =>
        this switch
        {
            Some<T>(var t) => new Some<R>(f(t)),
            None<T> _ => new None<R>(),
            _ => throw new NotSupportedException()
        };

    public Data<Option, T> Pure(T t) => 
        new Some<T>(t);
        

}


public sealed record None<T> : Option<T>
{
}

public sealed record Some<T>(T Value) : Option<T> { }

public interface Class<Data, Type, Kind>
    where Data : Data<Type, Kind> { }

public interface Monoid { }

public interface Monoid<Type, Kind> : Class<Data<Type, Kind>, Type, Kind>
{

}

public interface Functor<Type, Kind> : Class<Data<Type, Kind>, Type, Kind> 
{
    Data<Type, R> Map<R>(Func<Kind, R> f);

    /// <summary>
    /// Convenience overload to enable linq syntax
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    Data<Type, R> Select<R>(Func<Kind, R> f) => Map(f);
}

public interface Applicative<Type, Kind> : Functor<Type, Kind>
{
    Data<Type, Kind> Pure(Kind t);
    Data<Type, R> Apply<R>(Data<Type, Func<Kind, R>> f, Data<Type, Kind> t);
}

public interface Monad<Type, Kind> : Applicative<Type, Kind>
{
    Data<Type, Kind> Return(Kind t) => Pure(t);

    Data<Type, R> Bind<R>(Func<Kind, Data<Type, R>> f);

    /// <summary>
    /// Convenience overload to enable linq syntax
    /// </summary>
    /// <typeparam name="R"></typeparam>
    /// <param name="f"></param>
    /// <returns></returns>
    Data<Type, R> SelectMany<R>(Func<Kind, Data<Type, R>> f)
        => Bind(f); 
}

//public sealed record None<T> : Option<T>, Kind<Option, T>
//{
//    public T Value => throw new NotImplementedException();
//}

//public sealed record Some<T> : Option<T>, Kind<Option, T>
//{
//    public T Value => throw new NotImplementedException();
//}

//public abstract record Option<T>(T Value)
//    : Functor<Option<T>> 
//{

//    //public Option<R> Select(Func<T, R> f)
//    //    =>
//    //    this switch
//    //    {
//    //        Some<T>(var t) => new Some<R>(f(t)),
//    //        None<T> _ => new None<R>(),
//    //        _ => throw new NotSupportedException()
//    //    };
//    //public Option<R> Select<R>(Func<T, R> f) => throw new NotImplementedException();
//    //public K Select<K, R>(Func<T, R> f) where K : Option<R> => throw new NotImplementedException();
//    public Kind<Option<T>, R> Select<T1, R>(Func<T1, R> f, Kind<Option<T>, T1> kind) => throw new NotImplementedException();
//    //public RWitness Select<RWitness, R>(Func<T, R> f) where RWitness : Functor<RWitness, R> => throw new NotImplementedException();
//}

public static class Extensions
{


    public static Data<T, R> foo<T, K, R>(Functor<T, K> functor, Func<K, R> f)
            => from non in functor select f(non);

    public static void Bar()
    {
        var o = new None<int>();

        var m = foo(o, x => x++);

        if (m is None<int>) Console.WriteLine(m);
    }

}

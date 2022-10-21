using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Radix.Data;
using Radix.Math.Pure.Algebra.Structure;
using XPlot.Plotly;

namespace Radix.Tests;

public interface Data<Type, Kind>
{

}

public interface Option { }

public record Option<T> : Data<Option, T>, Functor<Option, T>
{
    public Data<Option, R> Select<R>(Func<T, R> f) =>
        this switch
        {
            Some<T>(var t) => new Some<R>(f(t)),
            None<T> _ => new None<R>(),
            _ => throw new NotSupportedException()
        };

}

public sealed record None<T> : Option<T> { }

public sealed record Some<T>(T Value) : Option<T> { }

public interface Class<Data, Type, Kind>
    where Data : Data<Type, Kind> { }

public interface Monoid { }

public interface Monoid<Type, Kind> : Class<Data<Type, Kind>, Type, Kind>
{

}

public interface Functor<Type, Kind> : Class<Data<Type, Kind>, Type, Kind> 
{
    Data<Type, R> Select<R>(Func<Kind, R> f);
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

    //public static Bar()
    //{
    //    var o = new None<int>();

    //    var m = foo(o, x => x++);

    //    if (m is None<int>) Console.WriteLine(m);
    //}

}

//namespace Radix.Tests;

//public interface Monad<Type, Kind> : Applicative<Type, Kind>
//{
//    Data<Type, Kind> Return(Kind t) => Pure(t);

//    Data<Type, R> Bind<R>(Func<Kind, Data<Type, R>> f);

//    /// <summary>
//    /// Convenience overload to enable linq syntax
//    /// </summary>
//    /// <typeparam name="R"></typeparam>
//    /// <param name="f"></param>
//    /// <returns></returns>
//    Data<Type, R> SelectMany<R>(Func<Kind, Data<Type, R>> f)
//        => Bind(f); 
//}

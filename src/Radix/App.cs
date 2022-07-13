//namespace Radix;

//public interface Type<T>
//{
//    static abstract Type<T> Inject(T t);
//    T Project();
//}

//public interface Functor<K, T>
//    where K : Type<T>
//{
//    static abstract Type<R> Map<R>(K type, Func<T, R> f);
//}

//public interface Applicative<K, T> : Functor<K, T>
//    where K : Type<T>
//{
//    static abstract Type<T> Return(T t);
//}

//public interface Monad<K, T> :
//    Applicative<K, T>
//    where K : Type<T>
//{
//    static abstract Type<R> Bind<KR, R>(K type, Func<T, Type<R>> f);
//}

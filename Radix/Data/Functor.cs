namespace Radix.Data;

public interface Functor<TWitness>
{
    Kind<TWitness, R> Select<T, R>(Func<T, R> f, Kind<TWitness, T> kind);

}

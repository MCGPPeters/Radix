namespace Radix.Math.Pure.Algebra.Structure;

public interface Monoid<A> : Semigroup<A>
{
    static abstract A Identity { get; }
}

namespace Radix.Math.Pure.Algebra.Structure;

public interface Group<A> : Monoid<A>
{
    static abstract Func<A, A> Invert { get; }
}

using Radix.Math.Pure.Algebra.Operations;

namespace Radix.Math.Pure.Algebra.Structure;

public interface Monoid<A, out B> : Semigroup<A, B> where B : Binary<A>
{
    A Identity { get; }
}

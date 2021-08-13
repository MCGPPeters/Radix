using Radix.Math.Pure.Algebra.Operations;

namespace Radix.Math.Pure.Algebra.Structure;

public interface Group<A, out B> :
    Monoid<A, B> where B : Binary<A>
{
    A Invert();
}

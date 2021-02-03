using Radix.Math.Pure.Algebra.Operations;

namespace Radix.Math.Pure.Algebra.Structure
{
    public interface Ring<A> :
        Monoid<A, Multiplication<A>>,
        Group<A, Addition<A>>
    {
    }
}

using Radix.Math.Pure.Algebra.Operations;

namespace Radix.Math.Pure.Algebra.Structure
{
    public interface Semigroup<A, out B> where B : Binary<A>
    {
        public B Combine { get; }
    }
}

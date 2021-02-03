using Radix.Math.Pure.Algebra.Structure;
using static Radix.Result.Extensions;

namespace Radix.Math.Pure.Algebra.Linear
{
    public class VectorSpace<T, D>
        where T : Field<T>
        where D : Dimension
    {
        public D Dimensionality { get; }

        public VectorSpace(D dimensionality)
        {
            Dimensionality = dimensionality;
        }

        public Result<Vector, Error> Create() => Error<Vector, Error>("");
    }

    public interface Vector
    {
    }

    public interface Dimension
    {
    }
}

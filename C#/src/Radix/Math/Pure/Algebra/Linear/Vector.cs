using Radix.Math.Pure.Algebra.Structure;
using static Radix.Result.Extensions;

namespace Radix.Math.Pure.Algebra.Linear
{
    public class VectorSpace<T>
    {
        public VectorSpace()
        {

        }

        public Result<Vector, Error> Create(Field<T>[] elements)
        {
            return Error<Vector, Error>("");
        }
    }

    public interface Vector
    {
    }

    public interface Dimension
    {
    }
}

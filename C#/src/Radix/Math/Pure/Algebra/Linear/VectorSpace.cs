using Radix.Data;
using Radix.Math.Pure.Algebra.Structure;
using Radix.Result;

namespace Radix.Math.Pure.Algebra.Linear
{
    public class VectorSpace<T>
        where T : Field<T>
    {
        public VectorSpace(uint dimensionality)
        {
            Dimensionality = dimensionality;
        }

        private uint Dimensionality { get; }

        public Result<Vector<T>, Error> Create(params T[] elements) =>
            (Dimensionality == elements.Length) switch
            {
                true => new Ok<Vector<T>, Error>(new Vector(elements)),
                _ => new Error<Vector<T>, Error>($"The number of elements does not match the dimensionality {Dimensionality.ToString()} of the vector space")
            };

        private record Vector : Vector<T>
        {
            public T[] Elements { get; }

            public Vector(params T[] elements)
            {
                Elements = elements;
            }
        }

    }
}

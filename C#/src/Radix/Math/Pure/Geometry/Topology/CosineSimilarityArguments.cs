using Radix.Validated;

namespace Radix.Math.Pure.Geometry.Topology
{

    public record CosineSimilarityArguments : SimilarityArguments<double[]>
    {


        private CosineSimilarityArguments(double[] first, double[] second)
        {
            First = first;
            Second = second;
        }

        public double[] First { get; }
        public double[] Second { get; }

        public static Validated<CosineSimilarityArguments> Create(double[] first, double[] second)
            => first.Length == second.Length
                ? Extensions.Valid(new CosineSimilarityArguments(first, second))
                : Extensions.Invalid<CosineSimilarityArguments>(
                    $"The operands {nameof(first)} and {nameof(second)} MUST have the same length");
    }
}

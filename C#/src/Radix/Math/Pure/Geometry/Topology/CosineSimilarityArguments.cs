using Radix.Data;

using static Radix.Control.Validated.Extensions;

namespace Radix.Math.Pure.Geometry.Topology;

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
            ? Valid(new CosineSimilarityArguments(first, second))
            : Invalid<CosineSimilarityArguments>(
                $"The operands {nameof(first)} and {nameof(second)} MUST have the same length");
}

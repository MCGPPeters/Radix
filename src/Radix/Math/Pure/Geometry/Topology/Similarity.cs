namespace Radix.Math.Pure.Geometry.Topology;

public delegate double Similarity<in T, TArguments>(T x) where T : SimilarityArguments<TArguments>;

public static class Similarity
{
    public static Similarity<CosineSimilarityArguments, double[]> Cosine =>
        cosineSimilarityArguments =>
        {
            double sum = 0;
            double p = 0;
            double q = 0;

            for (int i = 0; i < cosineSimilarityArguments.First.Length; i++)
            {
                sum += cosineSimilarityArguments.First[i] * cosineSimilarityArguments.Second[i];
                p += cosineSimilarityArguments.First[i] * cosineSimilarityArguments.First[i];
                q += cosineSimilarityArguments.Second[i] * cosineSimilarityArguments.Second[i];
            }

            double denominator = System.Math.Sqrt(p) * System.Math.Sqrt(q);
            return sum == 0
                ? 0
                : sum / denominator;
        };
}

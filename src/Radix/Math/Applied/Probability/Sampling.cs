namespace Radix.Math.Applied.Probability;

public static class Sampling
{
    /// <summary>
    ///   Scan a distribution for a target probability
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the random variable
    /// </typeparam>
    /// <param name="distribution">
    ///     The distribution
    /// </param>
    /// <param name="target">
    ///     The target probability
    /// </param>
    /// <returns>
    ///     A sequence of random values from the distribution
    /// </returns>
    public static IEnumerable<T> Scan<T>(this Distribution<T> distribution, Probability target)
    {
        Random? random = new();
        int maxValue = distribution.OutcomeProbabilities.Length;
        while (true)
        {
            int row = random.Next(0, maxValue);
            (Outcome<T> outcome, Probability probability) = distribution.OutcomeProbabilities[row];
            if (target <= probability)
            {
                yield return outcome;
            }
        }
    }

    /// <summary>
    ///    Choose a random value from a distribution
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the random variable
    /// </typeparam>
    /// <param name="distribution">
    ///     The distribution
    /// </param>
    /// <returns>
    ///     A random value from the distribution
    /// </returns>
    public static Random<T> Choose<T>(this Distribution<T> distribution)
    {
        Random? random = new();
        Probability probability = (Probability)random.NextDouble();
        // don't scan for a target larger than the maximum probability in the distribution
        Probability target = 
            probability <= distribution.Max 
                ? probability 
                : distribution.Max;
        Random<T> choose = Scan(distribution, target)
            .Select(x => new Random<T>(x))
            .First();
        return
            choose;
    }
}

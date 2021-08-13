namespace Radix.Math.Applied.Probability;

public static class Sampling
{
    public static IEnumerable<T> Scan<T>(this Distribution<T> distribution, Probability target) where T : notnull
    {
        var eventProbabilities = distribution.Value.ToList();
        Random? random = new();
        int maxValue = eventProbabilities.Count - 1;
        while (true)
        {
            int row = random.Next(0, maxValue);
            (Event<T> @event, Probability probability) = eventProbabilities[row];
            if (target <= probability)
            {
                yield return @event.Value;
            }
        }
    }

    public static Random<T> Choose<T>(this Distribution<T> distribution) where T : notnull
    {
        Random? random = new();
        Probability probability = new(random.NextDouble());
        // don't scan for a target larger than the maximum probability in the distribution
        Probability target = probability <= distribution.Max ? probability : distribution.Max;
        return
            Scan(distribution, target)
                .Select(x => new Random<T>(x))
                .First();
    }
}

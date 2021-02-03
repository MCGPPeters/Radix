using System;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Math.Applied.Probability
{
    public record Randomized<T>(T Value) : Alias<T>(Value);

    public static class Sampling
    {
        public static IEnumerable<T> Scan<T>(this Distribution<T> distribution, Probability target)
        {
            foreach ((Event<T> @event, Probability probability) in distribution.Value)
            {
                if (target <= probability)
                {
                    return Enumerable.Repeat(@event.Value, 1);
                }

                target = new Probability(target.Value - probability.Value);
            }

            return Enumerable.Empty<T>();
        }

        public static Randomized<T> Choose<T>(this Distribution<T> distribution)
        {
            Random? random = new Random();
            return
                Scan(distribution, new Probability(random.NextDouble()))
                    .Select(x => new Randomized<T>(x))
                    .First();
        }
    }
}

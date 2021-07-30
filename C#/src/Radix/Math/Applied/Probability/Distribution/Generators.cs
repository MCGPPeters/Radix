using Radix.Math.Applied.Probability;

namespace Radix.Math.Applied.Probability.Distribution
{
    public static class Generators
    {
        public static Distribution<T> Certainly<T>(Event<T> @event) where T : notnull
            => Distribution<T>.Return(@event);

        public static Distribution<T> Certainly<T>(T @event) where T : notnull
            => Distribution<T>.Return(@event);

        public static Distribution<T> Impossible<T>() where T : notnull
            => Distribution<T>.Impossible;
    }
}

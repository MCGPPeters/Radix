using System;

namespace Radix.Math.Applied.Probability.Distribution
{
    public static class Generators
    {
        public static Distribution<T> Certainly<T>(Event<T> @event)
            => Distribution<T>.Return(@event);

        public static Distribution<T> Impossible<T>()
            => Distribution<T>.Impossible;
    }
}

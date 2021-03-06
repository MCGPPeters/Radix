﻿using System;
using System.Collections.Generic;
using System.Linq;
using Radix.Math.Applied.Probability.Event;

namespace Radix.Math.Applied.Probability
{

    public record Distribution<T> : Alias<IEnumerable<(Event<T> @event, Probability probability)>> where T : notnull
    {
        private Distribution(IEnumerable<(Event<T> @event, Probability probability)> distribution) : base(distribution)
        {
        }

        public static Distribution<T> Impossible
            => new(Enumerable.Empty<(Event<T>, Probability)>());

        public static Spread<T> Uniform => Shape(_ => 1.0);

        public static Spread<T> Normal => Shape(x => NormalCurve(0.5, 0.5, x));

        internal static Distribution<T> Return(params Event<T>[] events) =>
            new(events.Select(@event => (@event, new Probability(1.0 / events.Length))));

        internal static Distribution<U> Bind<U>(Distribution<T> prior, Func<T, Distribution<U>> f) where U : notnull
        {
            IEnumerable<(Event<U> y, Probability)>? result = from Prior in prior.Value
                let x = Prior.@event
                let P = Prior.probability
                let posteriors = f(x).Value
                from Posterior in posteriors
                let y = Posterior.@event
                let Q = Posterior.probability
                select (y, new Probability(P * Q));
            return new Distribution<U>(result);
        }

        internal static Distribution<U> Map<U>(Distribution<T> distribution, Func<T, U> project) where U : notnull
        {
            IEnumerable<(Event<U>, Probability P)>? result = from d in distribution.Value
                let x = d.@event
                let P = d.probability
                let y = x.Map(project)
                select (new Event<U>(y), P);
            return new Distribution<U>(result);
        }

        internal static Probability Sum(Distribution<T> distribution)
        {
            double result = (from d in distribution.Value
                select d.probability.Value).Sum();
            return new Probability(result);
        }

        internal static Distribution<T> Scale(Distribution<T> distribution)
        {
            Probability? q = Sum(distribution);
            IEnumerable<(Event<T> @event, Probability probability)> d = distribution.Value;
            return new Distribution<T>(d.Select(x => (x.@event, new Probability(x.probability / q.Value))));
        }

        public static Spread<T> Shape(Func<double, double> f)
            =>
                xs =>
                {

                    switch (xs.Count())
                    {
                        case 0:
                            return Impossible;
                        default:
                            double incr = 1.0 / xs.Count() - 1.0;
                            IEnumerable<Probability>? probabilities =
                                _
                                    .Iterate(0.0, x => incr + x)
                                    .Select(f)
                                    .Select(p => new Probability(p));
                            IEnumerable<Event<T>>? events = xs.Select(x => new Event<T>(x));
                            IEnumerable<(Event<T> @event, Probability probability)>? d = events.Zip(probabilities, (@event, probability) => (@event, probability));
                            return Scale(new Distribution<T>(d));
                    }
                };

        /// <summary>
        ///     Normal curve / Gaussian
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="standardDeviation"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double NormalCurve(double mean, double standardDeviation, double x)
            => 1.0 / System.Math.Sqrt(2.0 * System.Math.PI) * System.Math.Exp(-1.0 / 2.0 * System.Math.Pow((x - mean) / standardDeviation, 2.0));
    }
}

using Radix.Control.Validated;
using Radix.Math.Applied.Probability.Event;

namespace Radix.Math.Applied.Probability.Distribution
{
    public static class Extensions
    {
        public static Distribution<U> Bind<T, U>(this Distribution<T> prior, Func<T, Distribution<U>> f) where U : notnull
        {
            IEnumerable<(Outcome<U> y, Probability)>? result = from Prior in prior.OutcomeProbabilities
                                                               let x = Prior.outcome
                                                               let P = Prior.probability
                                                               let posteriors = f(x)
                                                               from Posterior in posteriors.OutcomeProbabilities
                                                               let y = Posterior.outcome
                                                               let Q = Posterior.probability
                                                               select (y, (Probability)(P * Q));
            var newDistribution = Distribution<U>.Create(result.ToArray());
            return newDistribution switch
            {
                Valid<Distribution<U>>(var valid) => valid,
                Invalid<Distribution<U>>(var invalid) => throw new Exception(invalid.ToString())
            };

        }

        public static Distribution<U> SelectMany<T, U>(this Distribution<T> prior, Func<T, Distribution<U>> f)
            => Bind(prior, f);

        public static Distribution<U> Map<T, U>(Distribution<T> distribution, Func<T, U> project)
        {
            IEnumerable<(Outcome<U>, Probability P)>? result = from d in distribution.OutcomeProbabilities
                                                               let x = d.outcome
                                                               let P = d.probability
                                                               let y = x.Map(project)
                                                               select (y, P);
            var newDistribution = Distribution<U>.Create(result.ToArray());
            return newDistribution switch
            {
                Valid<Distribution<U>>(var valid) => valid,
                Invalid<Distribution<U>>(var invalid) => throw new Exception(invalid.ToString())
            };
        }

        public static Distribution<U> Select<T, U>(this Distribution<T> distribution, Func<T, U> project)
            => Map(distribution, project);

        public static Probability Sum<T>(Distribution<T> distribution)
        {
            double result = (from d in distribution.OutcomeProbabilities
                             select d.probability).Aggregate((a, b) => a + b);
            return (Probability)result;
        }

        public static Distribution<T> Scale<T>(this (Outcome<T> outcome, Probability probability)[] outcomeProbabilities)
        {
           double q = outcomeProbabilities.Sum(x => x.probability);
           var newOutcomeProbabilities =
               outcomeProbabilities.Select(x => (x.outcome, (Probability)(x.probability / q)));
            var newDistribution = Distribution<T>.Create(newOutcomeProbabilities.ToArray());
            return newDistribution switch
            {
                Valid<Distribution<T>>(var valid) => valid,
                Invalid<Distribution<T>>(var invalid) => throw new Exception(invalid.ToString())
            };
        }

        /// <summary>
        ///    Shape a distribution
        /// </summary>
        /// <param name="f">
        ///     A function that maps a number in the interval [0.0, 1.0] to a probability
        /// </param>
        /// <returns></returns>
        public static Spread<T> Shape<T>(Func<double, double> f)
            =>
                xs =>
                {
                    IEnumerable<T> ts = xs as T[] ?? xs.ToArray();
                    switch (ts.Count())
                    {
                        case 0:
                            return Distribution<T>.Impossible;
                        default:
                            double incr = 1.0 / ts.Count() - 1.0;
                            IEnumerable<Probability>? probabilities =
                                Prelude
                                    .Iterate(0.0, x => incr + x)
                                    .Select(f)
                                    .Select(p => (Probability)p);
                            IEnumerable<Outcome<T>>? outcomes = ts.Select(x => new Outcome<T>(x));
                            IEnumerable<(Outcome<T> outcome, Probability probability)>? d = outcomes.Zip(probabilities, (outcome, probability) => (outcome, probability));

                            var newDistribution = Distribution<T>.Create(d.ToArray().Scale());

                            return newDistribution switch
                            {
                                Valid<Distribution<T>>(var valid) => valid,
                                Invalid<Distribution<T>>(var invalid) => throw new Exception(invalid.ToString())
                            };
                    }
                };
    }
}

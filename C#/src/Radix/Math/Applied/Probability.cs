using System;
using System.Collections.Generic;
using System.Linq;
using static Radix.Validated.Extensions;

namespace Radix.Math.Applied.Probability
{
    public record Event<T>(T Value) : Alias<T>(Value);

    public static class EventExtensions
    {
        public static Event<U> Map<T, U>(this Event<T> e, Func<T, U> project)
         => new Event<U>(project(e.Value));

        public static Event<U> Select<T, U>(this Event<T> e, Func<T, U> project)
         => Map(e, project);
    }

    public record Probability : Alias<double>
    {
        internal Probability(double value) : base(value)
        {

        }

        public static Func<double, Validated<Probability>> Create =>
            value =>
                value switch
                {
                    >= 0.0 and <= 1.0 => Valid(new Probability(value)),
                    _ => Invalid<Probability>("The value of a probability should be in the interval [0.0, 1.0]")
                };

    }

    public delegate double Random<T>(T outcome);

    public record Expectation<T>(Random<T> Value) : Alias<Random<T>>(Value);

    namespace Radix.Math.Applied.Probability.Discrete
    {

    }

    public delegate Distribution<T> Spread<T>(IEnumerable<T> ts);

    public record Distribution<T> : Alias<IEnumerable<(Event<T> @event, Probability probability)>>
    {
        private Distribution(IEnumerable<(Event<T> @event, Probability probability)> distribution) : base(distribution)
        { }

        public static Distribution<T> Certainly(Event<T> @event)
            => new Distribution<T>(Enumerable.Repeat((@event, new Probability(1.0F)), 1));

        public static Distribution<T> Impossible
            => new Distribution<T>(Enumerable.Empty<(Event<T>, Probability)>());

        internal static Distribution<U> Bind<U>(Distribution<T> prior, Func<T, Distribution<U>> f)
        {
            var result = from Prior in prior.Value
                         let x = Prior.@event
                         let P = Prior.probability
                         let posteriors = f(x).Value
                         from Posterior in posteriors
                         let y = Posterior.@event
                         let Q = Posterior.probability
                         select (y, new Probability(P * Q));
            return new Distribution<U>(result);
        }

        internal static Distribution<U> Map<U>(Distribution<T> distribution, Func<T, U> project)
        {
            var result = from d in distribution.Value
                         let x = d.@event
                         let P = d.probability
                         let y = x.Map(project)
                         select (new Event<U>(y), P);
            return new Distribution<U>(result);
        }

        internal static Probability Sum(Distribution<T> distribution)
        {
            var result = (from d in distribution.Value
                          select d.probability.Value).Sum();
            return new Probability(result);
        }

        internal static Distribution<T> Scale(Distribution<T> distribution)
        {
            var q = Sum(distribution);
            var d = distribution.Value;
            return new Distribution<T>(d.Select(x => (x.@event, new Probability(x.probability / q))));
        }

        public static Spread<T> Shape(Func<double, double> f)
           =>
               xs =>
               {

                   switch (xs.Count())
                   {
                       case 0:
                           return Distribution<T>.Impossible;
                       default:
                           var incr = 1.0 / xs.Count() - 1.0;
                           var probabilities =
                               _
                               .Iterate(0.0, x => incr + x)
                               .Select(f)
                               .Select(p => new Probability(p));
                           var events = xs.Select(x => new Event<T>(x));
                           var d = Enumerable.Zip(events, probabilities, (@event, probability) => (@event, probability));
                           return Scale(new Distribution<T>(d));
                   }
               };

        public static Spread<T> Uniform => Shape(_ => 1.0);

        /// <summary>
        /// Normal curve / Gaussian
        /// </summary>
        /// <param name="mean"></param>
        /// <param name="standardDeviation"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        private static double NormalCurve(double mean, double standardDeviation, double x)
            => 1.0 / System.Math.Sqrt(2.0 * System.Math.PI) * System.Math.Exp((-1.0 / 2.0) * System.Math.Pow((x - mean) / standardDeviation, 2.0));

        public static Spread<T> Normal => Shape(x => NormalCurve(0.5, 0.5, x));
    }

    public static class DistributionExtentions
    {
        /// <summary>
        /// Monadic bind
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="prior"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Distribution<U> Bind<T, U>(this Distribution<T> prior, Func<T, Distribution<U>> f)
            => Distribution<T>.Bind(prior, f);

        /// <summary>
        /// For Linq support
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="prior"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        public static Distribution<U> SelectMany<T, U>(this Distribution<T> prior, Func<T, Distribution<U>> f)
            => prior.Bind(f);

        /// <summary>
        /// Functor map
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="distribution"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Distribution<U> Map<T, U>(this Distribution<T> distribution, Func<T, U> project)
            => Distribution<T>.Map(distribution, project);

        /// <summary>
        /// for Linq support
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="distribution"></param>
        /// <param name="project"></param>
        /// <returns></returns>
        public static Distribution<U> Select<T, U>(this Distribution<T> distribution, Func<T, U> project)
            => distribution.Map(project);

        public static Probability Sum<T, U>(this Distribution<T> distribution)
            => Distribution<T>.Sum(distribution);

        public static Distribution<T> Scale<T>(this Distribution<T> distribution)
            => Distribution<T>.Scale(distribution);
    }
}

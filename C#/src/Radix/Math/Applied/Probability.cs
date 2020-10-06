using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static Radix.Validated.Extensions;

namespace Radix.Math.Applied
{
    public record Event<T>(T Value) : Alias<T>(Value);

    public static class EventExtensions
    {
        public static Event<U> Map<T, U>(this Event<T> e, Func<T, U> project)
         => new Event<U>(project(e.Value));

        public static Event<U> Select<T, U>(this Event<T> e, Func<T, U> project)
         => Map(e, project);
    }

    public record Probability : Alias<float>
     {
        internal Probability(float value) : base(value)
        {

        }

        public static Func<float, Validated<Probability>> Create =>
            value =>
                value switch
                {
                    >= 0.0F and <= 1.0F => Valid(new Probability(value)),
                    _ => Invalid<Probability>("The value of a probability should be in the interval [0.0, 1.0]")
                };

    }

    public delegate float Random<T>(T outcome);

    public record Distribution<T> : Alias<IEnumerable<(Event<T>, Probability)>>
    {
        private Distribution(IEnumerable<(Event<T> @event, Probability probability)> distribution) : base(distribution)
        { }

        public static Distribution<T> Certainly(Event<T> @event) 
            => new Distribution<T>(Enumerable.Repeat((@event, new Probability(1.0F)), 1));

        public static Distribution<T> Impossible(Event<T> @event)
            => new Distribution<T>(Enumerable.Repeat((@event, new Probability(0.0F)), 1));

        internal static Distribution<U> Bind<U>(Distribution<T> prior, Func<T, Distribution<U>> f)
        {
            var result = from Prior in prior.Value
                         let x = Prior.Item1
                         let P = Prior.Item2
                         let posteriors = f(x).Value
                         from Posterior in posteriors
                         let y = Posterior.Item1
                         let Q = Posterior.Item2
                         select (y, new Probability(P * Q));
            return new Distribution<U>(result);
        }

        internal static Distribution<U> Map<U>(Distribution<T> distribution, Func<T, U> project)
        {
            var result = from d in distribution.Value
                         let x = d.Item1
                         let P = d.Item2
                         let y = x.Map(project)
                         select (new Event<U>(y), P);
            return new Distribution<U>(result);
        }
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

        public static Distribution<U> Select<T, U>(this Distribution<T> distribution, Func<T, U> project)
            => distribution.Map(project);
    }
}

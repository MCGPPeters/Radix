using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static Radix.Validated.Extensions;

namespace Radix.Math.Applied
{
    public record Event<T>(T Value) : Alias<T>(Value);

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

        public static Distribution<U> Bind<U>(Distribution<T> prior, Func<T, Distribution<U>> f)
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
    }

    public static class DistributionExtentions
    {

        public static Distribution<U> Bind<T, U>(this Distribution<T> prior, Func<T, Distribution<U>> f)
            => Distribution<T>.Bind(prior, f);
    }
}

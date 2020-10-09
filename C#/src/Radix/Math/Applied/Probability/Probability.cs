using System;
using System.Collections.Generic;
using static Radix.Validated.Extensions;

namespace Radix.Math.Applied.Probability
{

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

    public delegate Distribution<T> Spread<T>(IEnumerable<T> ts);
}

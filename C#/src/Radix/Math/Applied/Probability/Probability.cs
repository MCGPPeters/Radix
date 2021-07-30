using System;
using System.Collections.Generic;
using static System.Math;

namespace Radix.Math.Applied.Probability
{
    public readonly record struct Probability
    {
        private readonly double _p;

        public Probability(double p)
        {
            _p = p;
        }

        public static Func<double, Validated<Probability>> Create =>
            value =>
                value switch
                {
                    >= 0.0 and <= 1.0 => Valid(new Probability(value)),
                    _ => Invalid<Probability>("The value of a probability should be in the interval [0.0, 1.0]")
                };

        public static implicit operator double(Probability value) => value._p;

        public static Probability operator +(Probability p, Probability q) => new Probability(p._p + q._p);
    }

    public delegate Distribution<T> Spread<T>(IEnumerable<T> ts) where T : notnull;


}

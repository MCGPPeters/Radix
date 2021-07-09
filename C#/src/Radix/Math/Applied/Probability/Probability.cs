using System;
using System.Collections.Generic;
using static Radix.Validated.Extensions;
using static System.Math;

namespace Radix.Math.Applied.Probability
{

    public readonly struct Probability
    {
        public double Value { get; }

        public Probability(double value)
        {
            Value = value;

        }

        public static Func<double, Validated<Probability>> Create =>
            value =>
                value switch
                {
                    >= 0.0 and <= 1.0 => Valid(new Probability(value)),
                    _ => Invalid<Probability>("The value of a probability should be in the interval [0.0, 1.0]")
                };

        public static implicit operator Probability(double value) => new(value);
        public static implicit operator double(Probability value) => value.Value;

        public override bool Equals(object? obj) => Equals((Probability)obj);

        public bool Equals(Probability other)
        {
            double tolerance = Abs(Value * .00001);
            return Abs(Value - other.Value) <= tolerance;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(Probability left, Probability right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Probability left, Probability right)
        {
            return !(left == right);
        }
    }

    public delegate double Random<in T>(T outcome);

    public record Expectation<T>(Random<T> Value);

    public delegate Distribution<T> Spread<T>(IEnumerable<T> ts) where T : notnull;

    
}

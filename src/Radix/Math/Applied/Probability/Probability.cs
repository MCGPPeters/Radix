using Radix.Data;
using Radix.Generators.Attributes;
using static Radix.Control.Validated.Extensions;

namespace Radix.Math.Applied.Probability;

[Alias<double>]
public readonly partial record struct Probability : IComparable<Probability>
{
    public int CompareTo(Probability other) => Comparer<double>.Default.Compare(Value, other.Value);

    public static Func<double, Validated<Probability>> Create =>
        value =>
            value switch
            {
                >= 0.0 and <= 1.0 => Valid(new Probability(value)),
                _ => Invalid<Probability>(nameof(Probability), "The value of a probability should be in the interval [0.0, 1.0]")
            };

    public static Probability operator +(Probability p, Probability q) => new(p.Value + q.Value);
}

/// <summary>
/// The spread of a distribution is the difference between the largest and the smallest value in the distribution.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="ts"></param>
/// <returns></returns>
public delegate Distribution<T> Spread<T>(IEnumerable<T> ts);

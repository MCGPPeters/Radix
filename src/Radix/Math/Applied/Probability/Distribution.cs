using Radix.Data;
using Radix.Math.Applied.Optimization.Control.POMDP;
using Radix.Math.Applied.Probability;
using Radix.Math.Applied.Probability.Event;
using static Radix.Math.Applied.Probability.Distribution.Extensions;

namespace Radix.Math.Applied.Probability;

using static Radix.Control.Validated.Extensions;

/// <summary>
///    A distribution is a function that assigns a probability to each outcome.
///    The sum of the probabilities in a distribution must add up to 1.0.
/// </summary>
/// <typeparam name="T"></typeparam>
public record Distribution<T>
{
    public (Outcome<T> outcome, Probability probability)[] OutcomeProbabilities { get; }

    public static implicit operator (Outcome<T> outcome, Probability probability)[](Distribution<T> distribution) => distribution.OutcomeProbabilities;
    public static implicit operator Distribution<T>((Outcome<T> outcome, Probability probability)[] eventProbabilities) => new(eventProbabilities);

    public Probability Max { get; init; }

    private Distribution((Outcome<T> outcome, Probability probability)[] eventProbabilities) => OutcomeProbabilities = eventProbabilities;

    public static Validated<Distribution<T>> Create((Outcome<T> outcome, Probability probability)[] distribution)
    {
        double sum = distribution.Sum(x => x.probability);
        return System.Math.Abs(sum - 1.0) < 0.0001
            ? Valid(new Distribution<T>(distribution) { Max = distribution.Max(x => x.probability) })
            : Invalid<Distribution<T>>(nameof(Distribution),
                "The sum of probabilities in a distribution must add up to 1.0");
    }

    public static Distribution<T> Impossible
       => Return([]);

    public static Distribution<T> Return(params T[] outcomes) =>
       Uniform(outcomes);

    /// <summary>
    ///  Uniform distribution
    /// </summary>
    public static Spread<T> Uniform => xs => Shape<T>(x => 1.0)(xs);

    /// <summary>
    ///   Normal curve / Gaussian
    /// </summary>
    public static Spread<T> Normal(double mean, double standardDeviation) => xs => Shape<T>(x => NormalCurve(mean, standardDeviation, x))(xs);

    /// <summary>
    ///  Linear distribution
    /// </summary>
    /// <param name="c">
    ///     The slope of the line
    /// </param>
    /// <returns>
    ///     A linear distribution
    /// </returns>
    public static Spread<T> Linear(double c) => xs => Shape<T>(x => x * c)(xs);


    /// <summary>
    ///     Bernoulli distribution, a distribution with two outcomes
    ///     
    /// </summary>
    /// <param name="head">
    ///     The probability of the head
    /// </param>
    /// <param name="tail">
    ///     The probability of the tail
    /// </param>
    /// <returns>
    ///     A Bernoulli distribution
    /// </returns>
    public static Distribution<T> Bernoulli(T head, T tail) => Uniform([head, tail]);

    /// <summary>
    ///     Normal curve / Gaussian
    /// </summary>
    /// <param name="mean">μ</param>
    /// <param name="standardDeviation">σ</param>
    /// <param name="x"></param>
    /// <returns></returns>
    private static double NormalCurve(double mean, double standardDeviation, double x)
        => 1.0 / System.Math.Sqrt(2.0 * System.Math.PI) * System.Math.Exp(-1.0 / 2.0 * System.Math.Pow((x - mean) / standardDeviation, 2.0));

}

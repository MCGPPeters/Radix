using Radix.Data;
using Radix.Math.Applied.Probability.Event;

namespace Radix.Math.Applied.Probability;

using static Radix.Control.Validated.Extensions;

public record Distribution<T> where T : notnull
{
    public (Event<T> @event, Probability probability)[] EventProbabilities { get; }

    public static implicit operator (Event<T> @event, Probability probability)[](Distribution<T> distribution) => distribution.EventProbabilities;
    public static implicit operator Distribution<T>((Event<T> @event, Probability probability)[] eventProbabilities) => new(eventProbabilities);

    public Probability Max { get; init; }

    private Distribution((Event<T> @event, Probability probability)[] eventProbabilities) => EventProbabilities = eventProbabilities;

    public static Validated<Distribution<T>> Create((Event<T> @event, Probability probability)[] distribution)
        =>
            distribution.Sum(x => x.probability) == 1.0
            ? Valid(new Distribution<T>(distribution) { Max = distribution.Max(x => x.probability) })
            : Invalid<Distribution<T>>(nameof(Distribution), "The sum of probabilities in a distribution must add up to 1.0");


    public static Distribution<T> Impossible
        => Return(Array.Empty<T>());

    public static Spread<T> Uniform   => Shape(_ => 1.0);

    public static Spread<T> Normal => Shape(x => NormalCurve(0.5, 0.5, x));

    public static Spread<T> Linear(double c) => Shape(x => x * c);

    public static Distribution<T> Bernoulli(T head, T tail) => Uniform(new[] { head, tail });

    public static Distribution<T> Return(params Event<T>[] events) =>
       Uniform(events.Select(x => x.Value));

    public static Distribution<T> Return(params T[] events) =>
       Uniform(events);

    public static Distribution<U> Bind<U>(Distribution<T> prior, Func<T, Distribution<U>> f) where U : notnull
    {
        IEnumerable<(Event<U> y, Probability)>? result = from Prior in prior.EventProbabilities
                                                         let x = Prior.@event
                                                         let P = Prior.probability
                                                         let posteriors = f(x)
                                                         from Posterior in posteriors.EventProbabilities
                                                         let y = Posterior.@event
                                                         let Q = Posterior.probability
                                                         select (y, (Probability)(P * Q));
        return new Distribution<U>(result.ToArray());
    }

    public static Distribution<U> SelectMany<U>(Distribution<T> prior, Func<T, Distribution<U>> f) where U : notnull
        => Bind(prior, f);

    public static Distribution<U> Map<U>(Distribution<T> distribution, Func<T, U> project) where U : notnull
    {
        IEnumerable<(Event<U>, Probability P)>? result = from d in distribution.EventProbabilities
                                                         let x = d.@event
                                                         let P = d.probability
                                                         let y = x.Map(project)
                                                         select (y, P);
        return new Distribution<U>(result.ToArray());
    }

    public static Distribution<U> Select<U>(Distribution<T> distribution, Func<T, U> project) where U : notnull
        => Map(distribution, project);

    public static Probability Sum(Distribution<T> distribution)
    {
        double result = (from d in distribution.EventProbabilities
                         select d.probability).Aggregate((a, b) => a + b);
        return (Probability)result;
    }

    public static Distribution<T> Scale(Distribution<T> distribution)
    {
        Probability? q = Sum(distribution);
        return new Distribution<T>(distribution.EventProbabilities.Select(x => (x.@event, (Probability)(x.probability / q.Value))).ToArray());
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
                            Prelude
                                .Iterate(0.0, x => incr + x)
                                .Select(f)
                                .Select(p => (Probability)p);
                        IEnumerable<Event<T>>? events = xs.Select(x => new Event<T>(x));
                        IEnumerable<(Event<T> @event, Probability probability)>? d = events.Zip(probabilities, (@event, probability) => (@event, probability));
                        return Scale(new Distribution<T>(d.ToArray()));
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

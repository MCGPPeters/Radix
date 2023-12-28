namespace Radix.Math.Applied.Probability.Distribution;

public static class Generators
{
    /// <summary>
    ///   The certain distribution is a distribution that has only one event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="event"></param>
    /// <returns></returns>
    public static Distribution<T> Certainly<T>(in Event<T> @event)
        => Distribution<T>.Return(@event);

    /// <summary>
    ///    The certain distribution is a distribution that has only one event.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="event"></param>
    /// <returns></returns>
    public static Distribution<T> Certainly<T>(in T @event)
        => Certainly(new Event<T>(@event));

    /// <summary>
    ///    The impossible distribution is a distribution that has no events.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the random variable
    /// </typeparam>
    /// <returns>
    ///     The impossible distribution
    /// </returns>
    public static Distribution<T> Impossible<T>()
        => Distribution<T>.Impossible;
}

namespace Radix.Math.Applied.Probability.Distribution;

public static class Generators
{
    public static Distribution<T> Certainly<T>(in Event<T> @event) where T : notnull
        => Distribution<T>.Return(@event);

    public static Distribution<T> Certainly<T>(in T @event) where T : notnull
        => Distribution<T>.Return(@event);

    public static Distribution<T> Impossible<T>() where T : notnull
        => Distribution<T>.Impossible;
}

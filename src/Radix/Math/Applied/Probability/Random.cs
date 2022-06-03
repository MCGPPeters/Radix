namespace Radix.Math.Applied.Probability;

public record struct Random<T>(T Value)
{
    public static implicit operator T(Random<T> random) => random.Value;
    public static implicit operator Random<T>(T @event) => new(@event);
}

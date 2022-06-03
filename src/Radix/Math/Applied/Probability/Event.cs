namespace Radix.Math.Applied.Probability;

public record struct Event<T>(T Value)
{
    public static implicit operator T(Event<T> @event) => @event.Value;
    public static implicit operator Event<T>(T @event) => new(@event);
}

namespace Radix.Math.Applied.Probability;

/// <summary>
///   An event is a set of outcomes of an experiment (a subset of the sample space) to which a probability is assigned.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Value"></param>
public record struct Event<T>(T Value)
{
    public static implicit operator T(Event<T> @event) => @event.Value;
    public static implicit operator Event<T>(T @event) => new(@event);
}

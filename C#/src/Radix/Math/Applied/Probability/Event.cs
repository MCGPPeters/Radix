namespace Radix.Math.Applied.Probability
{
    public record Event<T>(T Value) : Alias<T>(Value)
    {
        public static implicit operator Event<T>(double value) => new(value);

        public static implicit operator T(Event<T> @event) => @event.Value;
    }
}

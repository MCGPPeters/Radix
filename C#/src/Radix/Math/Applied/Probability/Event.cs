namespace Radix.Math.Applied.Probability
{
    public record Event<T>(T Value) : Alias<T>(Value) where T : notnull
    {
        public static implicit operator T(Event<T> @event) => @event.Value;
    }
}

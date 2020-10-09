namespace Radix.Math.Applied.Probability
{
    public record Event<T>(T Value) : Alias<T>(Value);
}

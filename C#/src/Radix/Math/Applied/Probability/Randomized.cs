namespace Radix.Math.Applied.Probability
{
    public record Randomized<T>(T Value)  : Alias<T>(Value) where T : notnull;
}

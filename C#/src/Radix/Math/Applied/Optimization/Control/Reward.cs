namespace Radix.Math.Applied.Optimization.Control
{
    public record Reward(double Value) : Alias<double>(Value)
    {
        public static implicit operator Reward(double value) => new(value);
    }
}

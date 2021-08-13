namespace Radix.Math.Applied.Probability;

public record Random<T>(T Value) : Alias<T>(Value) where T : notnull;

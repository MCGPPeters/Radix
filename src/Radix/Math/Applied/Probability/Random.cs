namespace Radix.Math.Applied.Probability;

/// <summary>
/// A random variable is a variable whose value depends on the outcome of a random phenomenon.
/// </summary>
/// <typeparam name="T">
///     The type of the random variable
/// </typeparam>
/// <param name="Value">
///     The value of the random variable
/// </param>
public record Random<T>(T Value)
{
    public static implicit operator Random<T>(T value) => new(value);
    public static implicit operator T(Random<T> random) => random.Value;

}

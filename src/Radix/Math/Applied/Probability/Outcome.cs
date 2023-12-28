namespace Radix.Math.Applied.Probability;

/// <summary>
///   An outcome is the result of a single trial of an experiment.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Value"></param>
public record struct Outcome<T>(T Value)
{
    public static implicit operator T(Outcome<T> outcome) => outcome.Value;
    public static implicit operator Outcome<T>(T outcome) => new(outcome);
}

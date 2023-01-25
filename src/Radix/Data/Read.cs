using System.Globalization;

using static Radix.Control.Validated.Extensions;

namespace Radix.Data;

/// <summary>
/// Represents a type that can be parsed a produces a validated outcome
/// </summary>
/// <typeparam name="T"></typeparam>
public interface Read<out T> where T : IParsable<T>
{
    /// <summary>
    /// Parse the string <param name="s"></param>
    /// </summary>
    /// <param name="s"></param>
    /// <returns>A validated outcome</returns>
    static virtual Validated<T> Parse(string s)
        =>
            T.TryParse(s: s, provider: CultureInfo.InvariantCulture, result: out var result)
            ? Valid<T>(result)
            : Invalid<T>(new Reason(Title: s, $"The value '{s}' is not a valid '{typeof(T)}'"));

    /// <summary>
    /// Parse the string <param name="s"></param> and return a validated outcome. The validation message returned when
    /// parsing fails is <param name="validationErrorMessage"></param>
    /// </summary>
    /// <param name="s"></param>
    /// <param name="validationErrorMessage"></param>
    /// <returns></returns>
    static virtual Validated<T> Parse(string s, string validationErrorMessage)
        => T.TryParse(s: s, provider: CultureInfo.InvariantCulture, result: out var result)
            ? Valid<T>(result)
            : Invalid<T>(new Reason(Title: s, validationErrorMessage));
}

using System.Globalization;
using Radix.Control.Validated;

namespace Radix.Data;

/// <summary>
/// Represents a type that can be parsed a produces a validated outcome
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract record ParsableRead<T> : Read<T> where T : IParsable<T>
{
    /// <summary>
    /// Parse the string <param name="s"></param>
    /// </summary>
    /// <returns>A validated outcome</returns>
    public static Validated<T> Parse(string s)
        =>
            T.TryParse(s: s, provider: CultureInfo.InvariantCulture, result: out var result)
                ? Extensions.Valid(result)
                : Extensions.Invalid<T>(new Reason(Title: s, $"The value '{s}' is not a valid '{typeof(T)}'"));

    /// <summary>
    /// Parse the string <param name="s"></param> and return a validated outcome. The validation message returned when
    /// parsing fails is <param name="validationErrorMessage"></param>
    /// </summary>
    /// <returns></returns>
    public static Validated<T> Parse(string s, string validationErrorMessage)
        => T.TryParse(s: s, provider: CultureInfo.InvariantCulture, result: out var result)
            ? Extensions.Valid(result)
            : Extensions.Invalid<T>(new Reason(Title: s, validationErrorMessage));

}


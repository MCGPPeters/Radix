namespace Radix.Data.String.Validity;

using System;
using static Radix.Control.Validated.Extensions;

public class ContainsOnlyAlphaNumericsHyphensAndDots : Validity<string>
{
    public static Func<string, Func<string, Validated<string>>> Validate =>
        name =>
            value =>
                value.Length > 0 && value.All(s => Char.IsLetterOrDigit(s) || Char.Equals(s, '-') || Char.Equals(s, '.'))
                ? Valid(value)
                : Invalid<string>(name, $"The value of '{name}' may only container alphanumeric characters, hyphens and dots. Actual value '{value}'");
}

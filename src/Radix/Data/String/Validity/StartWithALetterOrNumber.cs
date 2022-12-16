namespace Radix.Data.String.Validity;

using System;
using static Radix.Control.Validated.Extensions;

public class StartsWithALetterOrNumber : Validity<string>
{
    public static Func<string, Func<string, Validated<string>>> Validate =>
        name =>
            value =>
                value.Length > 0 && (Char.IsLetter(value[0]) || Char.IsNumber(value[0]))
                ? Valid(value)
                : Invalid<string>($"The last character of '{name}' must be a letter or a number. Actual value '{value}'");
}

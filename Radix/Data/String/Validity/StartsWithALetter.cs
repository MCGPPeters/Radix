namespace Radix.Data.String.Validity;

using System;
using static Radix.Control.Validated.Extensions;

public class StartsWithALetter : Validity<string>
{
    public static Func<string, Func<string, Validated<string>>> Validate =>
        name =>
            value =>
                value.Length > 0 && Char.IsLetter(value[0])
                ? Valid(value)
                : Invalid<string>(name, $"The first character of '{name}' must be a letter. Actual value '{value}'");
}

namespace Radix.Data.String.Validity;

using System;
using System.Net.Http.Headers;
using static Radix.Control.Validated.Extensions;

public class ContainsOnlyAlphaNumericsAndHyphens : Validity<string>
{
    public static Func<string, Func<string, Validated<string>>> Validate =>
        name =>
            value =>
                value.Length > 0 && (value.All(s => Char.IsLetterOrDigit(s) || Char.Equals(s, '-')))
                ? Valid(value)
                : Invalid<string>(name, $"The value of '{name}' may only container alphanumeric characters and hyphens. Actual value '{value}'");
}

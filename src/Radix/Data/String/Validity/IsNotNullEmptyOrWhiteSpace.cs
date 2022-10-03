namespace Radix.Data.String.Validity;

using System;
using static Radix.Control.Validated.Extensions;

public class IsNotNullEmptyOrWhiteSpace : Validity<string>
{
    public static Func<string, Func<string, Validated<string>>> Validate =>
        name =>
            value =>
                string.IsNullOrWhiteSpace(value)
                ? Invalid<string>($"The value for '{name}' may not be null, empty or whitespace.")
                : Valid(value);
}

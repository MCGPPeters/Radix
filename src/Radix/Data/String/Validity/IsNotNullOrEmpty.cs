﻿namespace Radix.Data.String.Validity;

using static Radix.Control.Validated.Extensions;

public class IsNotNullOrEmpty : Validity<string>
{
    public static Validated<string> Validate(string name, string value) =>
        string.IsNullOrEmpty(value)
            ? Invalid<string>($"The value for the string '{name}' may not be null or empty")
            : Valid(value);
    public static Validated<string> Validate(string value) =>
        string.IsNullOrEmpty(value)
            ? Invalid<string>("The string may not be null or empty")
            : Valid(value);
}
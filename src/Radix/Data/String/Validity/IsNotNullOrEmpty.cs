namespace Radix.Data.String.Validity;

using static Radix.Control.Validated.Extensions;

public class IsNotNullOrEmpty : Validity<string>
{
    public static Validated<string> Validate(string name, string? value) =>
        string.IsNullOrEmpty(value)
            ? Invalid<string>($"'{name}' must have a value")
            : Valid(value);
    public static Validated<string> Validate(string? value) =>
        string.IsNullOrEmpty(value)
            ? Invalid<string>("The string must have a value")
            : Valid(value);
}

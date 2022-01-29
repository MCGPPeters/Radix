namespace Radix.Data.String.Validity;

using static Radix.Control.Validated.Extensions;

public class IsNotNullEmptyOrWhiteSpace : Validity<string>
{
    public static Validated<string> Validate(string value, string validationErrorMessage) =>
        string.IsNullOrWhiteSpace(value)
            ? Invalid<string>($"{validationErrorMessage}. The string may not be null, empty or whitespace")
            : Valid(value);

    public static Validated<string> Validate(string value) =>
        string.IsNullOrWhiteSpace(value)
            ? Invalid<string>("The string may not be null, empty or whitespace")
            : Valid(value);
}

namespace Radix.Data.String.Validity;

public class IsNotNullOrEmpty : Validity<string>
{
    public static Validated<string> Validate(string value, string validationErrorMessage) => string.IsNullOrEmpty(value) ? Invalid<string>($"{validationErrorMessage}. The string may not be null or empty") : Valid(value);
}

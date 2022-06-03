namespace Radix.Data.Decimal;

using static Radix.Control.Validated.Extensions;

public class Read : Read<decimal>
{
    public static Validated<decimal> Parse(string s) =>
        Parse(s, $"The value {s} is not a valid integer");

    public static Validated<decimal> Parse(string s, string validationErrorMessage) =>
        decimal.TryParse(s, out decimal i)
            ? Valid(i)
            : Invalid<decimal>(validationErrorMessage);
}

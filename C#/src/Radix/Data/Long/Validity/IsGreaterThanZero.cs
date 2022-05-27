namespace Radix.Data.Long.Validity;

using static Radix.Control.Validated.Extensions;

public class IsGreaterThanZero : Validity<long>
{
    public static Validated<long> Validate(string name, long value) =>
        value > 0
        ? Valid(value)
        : Invalid<long>($"The value for '{name}' must be larger than 0");
    public static Validated<long> Validate(long value) =>
        value > 0
        ? Valid(value)
        : Invalid<long>("The value must be larger than 0");
}

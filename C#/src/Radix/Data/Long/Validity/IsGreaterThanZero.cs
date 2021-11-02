using Radix.Data;

namespace Radix.Data.Long.Validity;

public class IsGreaterThanZero : Validity<long>
{
    public static Validated<long> Validate(long value, string validationErrorMessage) =>
        value > 0
        ? Valid(value)
        : Invalid<long>($"{validationErrorMessage}. The value must be larger than 0");
    public static Validated<long> Validate(long value) =>
        value > 0
        ? Valid(value)
        : Invalid<long>("The value must be larger than 0");
}

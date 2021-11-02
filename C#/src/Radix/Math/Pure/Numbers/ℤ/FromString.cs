using Radix.Data;

namespace Radix.Math.Pure.Numbers.ℤ;

public class FromString : FromString<int>
{
    public static Validated<int> Parse(string s) =>
        int.TryParse(s, out var i)
            ? Valid(i)
            : Invalid<int>($"The string '{s}' can not be parsed. The value is not a valid integer");
    public static Validated<int> Parse(string s, string validationErrorMessage) =>
                int.TryParse(s, out var i)
            ? Valid(i)
            : Invalid<int>($"{validationErrorMessage}. The string '{s}' can not be parsed. The value is not a valid integer");
}

using Radix.Data;

using static Radix.Control.Validated.Extensions;

namespace Radix.Math.Pure.Numbers.ℤ;

public class Read : Read<int>
{
    public static Validated<int> Parse(string s) =>
        int.TryParse(s, out int i)
            ? Valid(i)
            : Invalid<int>($"The string '{s}' can not be parsed. The value is not a valid integer");
    public static Validated<int> Parse(string s, string validationErrorMessage) =>
                int.TryParse(s, out int i)
            ? Valid(i)
            : Invalid<int>($"{validationErrorMessage}. The string '{s}' can not be parsed. The value is not a valid integer");
}

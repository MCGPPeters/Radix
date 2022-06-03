namespace Radix.Data.Int.Validity;

using static Radix.Control.Validated.Extensions;
public class IsGreaterOrEqualToZero : Validity<int>
{
    public static Validated<int> Validate(int value) =>
        value < 0
            ? Invalid<int>($"The value must be greater or equal to 0 (zero)")
            : Valid(value);

    public static Validated<int> Validate(string name, int value) =>
        value < 0
            ? Invalid<int>($"The value for '{name}' must be greater or equal to 0 (zero)")
            : Valid(value);
}


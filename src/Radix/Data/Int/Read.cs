namespace Radix.Data.Int;

using static Radix.Control.Validated.Extensions;
public class Read : Read<int>
{
    public static Validated<int> Parse(string s) =>
        Parse(s, $"The value {s} is not a valid integer");

    public static Validated<int> Parse(string s, string validationErrorMessage) =>
        int.TryParse(s, out int i)
            ? Valid(i)
            : Invalid<int>(validationErrorMessage);
}


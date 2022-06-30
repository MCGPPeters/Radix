using static Radix.Control.Validated.Extensions;

namespace Radix.Data.Guid;

public class Read : Read<System.Guid>
{
    public static Validated<System.Guid> Parse(string s) =>
        Parse(s, $"The value {s} is not a valid Guid");

    public static Validated<System.Guid> Parse(string s, string validationErrorMessage) =>
        System.Guid.TryParse(s, out System.Guid i)
            ? Valid(i)
            : Invalid<System.Guid>(validationErrorMessage);
}

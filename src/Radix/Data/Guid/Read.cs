using static Radix.Control.Validated.Extensions;
public class Read : Read<Guid>
{
    public static Validated<Guid> Parse(string s) =>
        Parse(s, $"The value {s} is not a valid Guid");

    public static Validated<Guid> Parse(string s, string validationErrorMessage) =>
        Guid.TryParse(s, out Guid i)
            ? Valid(i)
            : Invalid<Guid>(validationErrorMessage);
}

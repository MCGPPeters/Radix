namespace Radix.Validated;

public record struct NonEmptyString
{
    public string Value { get; }

    private NonEmptyString(string value)
    {
        Value = value;
    }

    public static implicit operator string(NonEmptyString nonEmptyString) => nonEmptyString.Value;

    public static Validated<NonEmptyString> Create(string value, string errorMessage)
        => value.IsNotNullNorEmpty(errorMessage)
            switch
        {
            Valid<string>(var s) => Valid(new NonEmptyString(s)),
            _ => Invalid<NonEmptyString>(errorMessage)
        };
}

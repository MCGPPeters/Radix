namespace Radix.Validated;

public record NonEmptyString : Alias<string>
{
    private NonEmptyString(string value) : base(value) { }

    public static Validated<NonEmptyString> Create(string value, string errorMessage)
        => value.IsNotNullNorEmpty(errorMessage)
            switch
        {
            Valid<string>(var s) => Valid(new NonEmptyString(s)),
            _ => Invalid<NonEmptyString>(errorMessage)
        };
}

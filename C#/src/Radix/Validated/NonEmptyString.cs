using static Radix.Validated.Extensions;

namespace Radix.Validated
{
    public record NonEmptyString(string Value) : Alias<string>(Value)
    {
        public static Validated<NonEmptyString> Create(string value, string errorMessage)
            => value.IsNotNullNorEmpty(errorMessage)
                switch
                {
                    Valid<string>(var s) => Valid(new NonEmptyString(s)),
                    _ => Invalid<NonEmptyString>(errorMessage)
                };
    }
}

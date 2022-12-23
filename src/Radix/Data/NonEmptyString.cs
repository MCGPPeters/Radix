using Radix.Control.Validated;

using static Radix.Control.Validated.Extensions;

namespace Radix.Data;

public record struct NonEmptyString
{
    public string Value { get; }

    private NonEmptyString(string value) => Value = value;

    public static implicit operator string(NonEmptyString nonEmptyString) => nonEmptyString.Value;

    public static Validated<NonEmptyString> Create(string value, string errorMessage)
        => string.IsNullOrEmpty(errorMessage)
            switch
            {
                false => Valid(new NonEmptyString(value)),
                _ => Invalid<NonEmptyString>(nameof(NonEmptyString), errorMessage)
            };
}

using System.Diagnostics.CodeAnalysis;
using static Radix.Validated.Extensions;

namespace Radix.Validated
{
    public readonly struct NonEmptyString : Value<string>
    {
        private NonEmptyString(string value) => Value = value;

        public string Value { get; }

        public static Validated<NonEmptyString> Create(string value, string errorMessage) => value.IsNotNullNorEmpty(errorMessage) switch
        {
            Valid<string>(var s) => Valid(new NonEmptyString(s)),
            _ => Invalid<NonEmptyString>(errorMessage)
        };


        public int CompareTo([AllowNull]NonEmptyString other) => Value.CompareTo(other.Value);

        public bool Equals([AllowNull]NonEmptyString other) => Value.Equals(other.Value);

        public static implicit operator string(NonEmptyString nonEmptyString) => nonEmptyString.Value;

        public void Deconstruct(out string value) => value = Value;
    }
}

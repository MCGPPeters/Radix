using System.IO;
using static Radix.Validated.Extensions;
using static System.String;

namespace Radix.Validated
{
    public static class StringExtensions
    {
        public static Validated<string> IsNotNullNorEmpty(this string? subject, string errorMessage)
        {
            return !IsNullOrEmpty(subject)
                ? Valid(subject)
                : Invalid<string>(new[] {errorMessage});
        }

        public static Validated<string> IsPathFullyQualified(this string? subject, string errorMessage)
        {
            return !IsNullOrEmpty(subject) && Path.IsPathFullyQualified(subject)
                ? Valid(subject)
                : Invalid<string>(new[] {errorMessage});
        }
    }
}

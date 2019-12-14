﻿using System.IO;
using static System.String;
using static Radix.Validated.Extensions;

namespace Radix.Validated
{
    public static class StringExtensions
    {
        public static Validated<string> IsNotNullNorEmpty(this string? subject, string errorMessage) =>
            !IsNullOrEmpty(subject)
                ? Valid(subject)
                : Invalid<string>(new[] { errorMessage });

        public static Validated<string> IsPathFullyQualified(this string? subject, string errorMessage) =>
            !IsNullOrEmpty(subject) && Path.IsPathFullyQualified(subject)
                ? Valid(subject)
                : Invalid<string>(new[] { errorMessage });
    }
}
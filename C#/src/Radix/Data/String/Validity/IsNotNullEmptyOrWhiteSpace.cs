﻿namespace Radix.Data.String.Validity;

public class IsNotNullEmptyOrWhiteSpace : Validity<string>
{
    public static Validated<string> Validate(string value, string validationErrorMessage) => string.IsNullOrWhiteSpace(value) ? Invalid<string>($"{validationErrorMessage}. The string may not be null, empty or whitespace") : Valid(value);
}

namespace Radix.Data.String.Validity;

using System;
using System.Xml.Linq;
using static Radix.Control.Validated.Extensions;

public class IsNotNullOrEmpty : Validity<string>
{
    public static Func<string, Func<string, Validated<string>>> Validate =>
        name =>
            value => 
                string.IsNullOrEmpty(value)
                    ? Invalid<string>($"'{name}' must have a value")
                    : Valid(value);
}

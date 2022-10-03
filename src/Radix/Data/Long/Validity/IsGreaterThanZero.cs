namespace Radix.Data.Long.Validity;

using System;
using System.Xml.Linq;
using static Radix.Control.Validated.Extensions;

public class IsGreaterThanZero : Validity<long>
{
    public static Func<string, Func<long, Validated<long>>> Validate =>
        name =>
            value =>
                value < 0
                        ? Invalid<long>($"The value for '{name}' must be greater or equal to 0 (zero)")
                        : Valid(value);



}

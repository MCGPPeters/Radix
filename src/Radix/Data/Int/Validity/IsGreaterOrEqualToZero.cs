namespace Radix.Data.Int.Validity;

using System;
using static Radix.Control.Validated.Extensions;
public class IsGreaterOrEqualToZero : Validity<int>
{
    public static Func<string, Func<int, Validated<int>>> Validate =>
        name =>
            value =>
                value < 0
                ? Invalid<int>($"The value for '{name}' must be greater or equal to 0 (zero)")
                : Valid(value);
       
}


namespace Radix.Data.Number.Validity;

using System;
using System.Numerics;
using static Radix.Control.Validated.Extensions;
public class IsGreaterOrEqualToZero<T> : Validity<T>
    where T : INumber<T>
{
    public static Func<string, Func<T, Validated<T>>> Validate =>
        name =>
            value =>
                value < T.Zero
                ? Invalid<T>(name, $"The value for '{name}' must be greater or equal to 0 (zero)")
                : Valid(value);

}

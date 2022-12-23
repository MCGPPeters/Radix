namespace Radix.Data.Double.Validity;

using System;
using static Radix.Control.Validated.Extensions;

public class InUnitInterval : Validity<double>
{
    public static Func<string, Func<double, Validated<double>>> Validate =>
        name =>
            value =>
                value switch
                {
                    >= 0.0 and <= 1.0 => Valid(value),
                    _ => Invalid<double>(new Reason(name, new[] { $"The value for '{name}' has to be a value in the interval [0, 1] but is '{value}'" }))
                };
        
}

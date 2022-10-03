namespace Radix.Data.Double.Validity;

using static Radix.Control.Validated.Extensions;

internal class InUnitInterval : Validity<double>
{
    public static Validated<double> Validate(double value) =>
        value switch
        {
            >= 0.0 and <= 1.0 => Valid(value),
            _ => Invalid<double>($"The discount factor has to be a value in the interval [0, 1] but is '{value}'")
        };

    public static Validated<double> Validate(string name, double value) =>
        value switch
        {
            >= 0.0 and <= 1.0 => Valid(value),
            _ => Invalid<double>($"The value for '{name}' has to be a value in the interval [0, 1] but is '{value}'")
        };
}

namespace Radix.Math.Applied.Optimization.Control;

[Alias<double>]
public partial struct DiscountFactor
{
    public static Validated<DiscountFactor> Create(double value) =>
        value switch
        {
            >= 0.0 and <= 1.0 => Valid<DiscountFactor>(value),
            _ => Invalid<DiscountFactor>($"The discount factor has to be a value in the interval [0, 1] but is '{value}'")
        };

}

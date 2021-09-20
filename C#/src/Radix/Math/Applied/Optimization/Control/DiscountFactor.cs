namespace Radix.Math.Applied.Optimization.Control;

public record struct DiscountFactor(double Value) : Alias<DiscountFactor, double>
{
    public static Validated<DiscountFactor> Create(double value) =>
        value switch
        {
            >= 0.0 and <= 1.0 => Valid<DiscountFactor>(value),
            _ => Invalid<DiscountFactor>($"The discount factor has to be a value in the interval [0, 1] but is '{value}'")
        };

    public static implicit operator double(DiscountFactor discountFactor) => discountFactor.Value;
    public static implicit operator DiscountFactor(double discountFactor) => new(discountFactor);

}

namespace Radix.Web.Css.Data.Dimensions;

public record Angle<T> : Dimension<T> where T : Literal<T>, Units.Angle.Unit<T>
{
    public Number Quantity { get; init; }
}

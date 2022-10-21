namespace Radix.Web.Css.Data.Dimensions;

public record Time<T> : Dimension<T>
    where T : Literal<T>, Units.Time.Unit<T>
{
    public Number Quantity { get; init; }
}

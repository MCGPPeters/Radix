namespace Radix.Web.Css.Data.Dimensions;

public record Resolution<T> : Dimension<T>
    where T : Units.Resolution.Unit<T>, Literal<T>
{
    public Number Number { get; init; }
}

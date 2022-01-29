namespace Radix.Web.Css.Data.Dimensions;

public record Time : Dimension<Units.Time.Unit>
{
    public Number Number { get; init; }
}

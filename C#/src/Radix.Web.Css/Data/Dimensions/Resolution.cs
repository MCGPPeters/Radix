namespace Radix.Web.Css.Data.Dimensions;

public record Resolution : Dimension<Units.Resolution.Unit>
{
    public Number Number { get; init; }
}

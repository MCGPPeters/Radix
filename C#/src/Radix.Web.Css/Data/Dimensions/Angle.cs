namespace Radix.Web.Css.Data.Dimensions;

public record Angle : Dimension<Units.Angle.Unit>
{
    public Number Number { get; init; }
}

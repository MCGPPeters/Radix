namespace Radix.Web.Css.Data.Dimensions;

public record RelativeLength<T> : Length<T>
    where T : Units.Length.Relative.Unit<T>, Literal<T>
{
    public RelativeLength(Number number)
    {
        Quantity = number;
    }

    public Number Quantity { get; init; }
}

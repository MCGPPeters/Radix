namespace Radix.Web.Css.Data.Dimensions;

public record RelativeLength<T> : Length<T>
    where T : Units.Length.Relative.Unit<T>, Literal<T>
{
    public RelativeLength(Number number)
    {
        Number = number;
    }

    public Number Number { get; init; }
}

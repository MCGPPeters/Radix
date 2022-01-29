namespace Radix.Web.Css.Data;

public struct Percentage
{
    public Percentage(Number number) => Number = number;

    public Number Number { get; init; }
}

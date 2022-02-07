namespace Radix.Web.Css.Data;

public struct Percentage : Value
{
    public Percentage(Number number) => Number = number;

    public Number Number { get; init; }
}

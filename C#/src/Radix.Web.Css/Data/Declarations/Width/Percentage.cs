namespace Radix.Web.Css.Data.Declarations.Width;

public struct Percentage : Declaration<Data.Percentage>
{
    public Properties.Values.Width Property { get; init; }
    public Properties.Width.Value<Data.Percentage> Value { get; init; }
}

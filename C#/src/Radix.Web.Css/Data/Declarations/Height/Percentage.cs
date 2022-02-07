namespace Radix.Web.Css.Data.Declarations.Height;

public struct Percentage : Declaration<Data.Percentage>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Data.Percentage> Value { get; init; }
}

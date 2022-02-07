namespace Radix.Web.Css.Data.Declarations.Width;

public partial struct initial : Declaration<Keywords.initial>
{
    public Properties.Values.Width Property { get; init; }
    public Properties.Width.Value<Keywords.initial> Value { get; init; }
}

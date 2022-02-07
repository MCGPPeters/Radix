namespace Radix.Web.Css.Data.Declarations.Height;

public partial struct initial : Declaration<Keywords.initial>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Keywords.initial> Value { get; init; }
}

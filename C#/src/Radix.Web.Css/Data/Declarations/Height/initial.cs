namespace Radix.Web.Css.Data.Declarations.Height;

public partial struct initial : Declaration<Global.initial>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Global.initial> Value { get; init; }
}

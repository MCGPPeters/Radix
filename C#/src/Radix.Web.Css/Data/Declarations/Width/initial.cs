namespace Radix.Web.Css.Data.Declarations.Width;

public partial struct initial : Declaration<Global.initial>
{
    public Properties.Values.Width Property { get; init; }
    public Properties.Width.Value<Global.initial> Value { get; init; }
}

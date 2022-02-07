namespace Radix.Web.Css.Data.Declarations.Height;

public partial struct unset : Declaration<Global.unset>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Global.unset> Value { get; init; }
}

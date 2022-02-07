namespace Radix.Web.Css.Data.Declarations.Width;

public partial record struct unset : Declaration<Global.unset>
{
    public Properties.Values.Width Property { get ; init; }
    public Properties.Width.Value<Global.unset> Value { get; init; }
}

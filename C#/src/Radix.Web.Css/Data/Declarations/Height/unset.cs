namespace Radix.Web.Css.Data.Declarations.Height;

public partial struct unset : Declaration<Keywords.unset>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Keywords.unset> Value { get; init; }
}

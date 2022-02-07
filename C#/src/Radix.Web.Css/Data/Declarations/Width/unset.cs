namespace Radix.Web.Css.Data.Declarations.Width;

public partial record struct unset : Declaration<Keywords.unset>
{
    public Properties.Values.Width Property { get ; init; }
    public Properties.Width.Value<Keywords.unset> Value { get; init; }
}

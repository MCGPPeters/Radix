namespace Radix.Web.Css.Data.Declarations.Width;

public partial record unset : Declaration<Keywords.unset>
{
    public new Properties.Width.Value<Keywords.unset>? Value { get; init; }
}

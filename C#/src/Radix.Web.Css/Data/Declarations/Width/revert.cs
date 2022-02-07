namespace Radix.Web.Css.Data.Declarations.Width;

public partial struct revert : Declaration<Global.revert>
{
    public Properties.Values.Width Property { get; init ; }
    public Properties.Width.Value<Global.revert> Value { get; init; }
}

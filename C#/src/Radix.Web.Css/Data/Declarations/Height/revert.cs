namespace Radix.Web.Css.Data.Declarations.Height;

public partial struct revert : Declaration<Global.revert>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Global.revert> Value { get; init; }
}

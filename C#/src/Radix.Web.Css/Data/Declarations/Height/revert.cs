namespace Radix.Web.Css.Data.Declarations.Height;

public partial struct revert : Declaration<Keywords.revert>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Keywords.revert> Value { get; init; }
}

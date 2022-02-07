namespace Radix.Web.Css.Data.Declarations.Height;

public partial struct auto : Declaration<Properties.Width.auto>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Properties.Width.auto> Value { get; init; }
}

namespace Radix.Web.Css.Data.Declarations.Width;

public partial struct auto : Declaration<Properties.Width.auto>
{
    public Properties.Values.Width Property { get; init; }
    public Properties.Width.Value<Properties.Width.auto> Value { get; init; }
}

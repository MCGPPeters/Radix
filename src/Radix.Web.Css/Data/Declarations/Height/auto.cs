namespace Radix.Web.Css.Data.Declarations.Height;

public partial record auto : Declaration<Properties.Width.auto>
{
    public new Properties.Height.Value<Properties.Width.auto>? Value { get; init; }
}

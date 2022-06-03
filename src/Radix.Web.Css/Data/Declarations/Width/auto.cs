namespace Radix.Web.Css.Data.Declarations.Width;

public record auto : Declaration<Properties.Width.auto>
{
    public Properties.Width.Value<Properties.Width.auto> Value { get; init; }
}

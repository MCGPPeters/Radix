namespace Radix.Web.Css.Data.Declarations.Height;

public record Percentage : Declaration<Data.Percentage>
{
    public Properties.Height.Value<Data.Percentage> Value { get; init; }
}

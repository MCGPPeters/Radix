namespace Radix.Web.Css.Data.Declarations.PaddingTop;

public record Percentage : Declaration<Data.Percentage>
{
    public Properties.PaddingTop.Value<Data.Percentage> Value { get; init; }
}

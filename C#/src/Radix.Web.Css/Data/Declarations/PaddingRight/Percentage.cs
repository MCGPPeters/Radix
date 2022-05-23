namespace Radix.Web.Css.Data.Declarations.PaddingRight;

public record Percentage : Declaration<Data.Percentage>
{
    public Properties.PaddingRight.Value<Data.Percentage> Value { get; init; }
}

namespace Radix.Web.Css.Data.Declarations.PaddingBottom;

public record Percentage : Declaration<Data.Percentage>
{

    public new Properties.PaddingBottom.Value<Data.Percentage>? Value { get; init; }
}

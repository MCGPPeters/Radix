namespace Radix.Web.Css.Data.Declarations.PaddingLeft;

public record Percentage : Declaration<Data.Percentage>, Declaration
{
    public Data.Percentage Value { get; init; }
}

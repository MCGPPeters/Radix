namespace Radix.Web.Css.Data.Declarations.Width;

public record Percentage : Declaration<Data.Percentage>
{
    public new Properties.Values.Width Property { get; init; }
    public new Properties.Width.Value<Data.Percentage>? Value { get; init; }
}

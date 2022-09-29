namespace Radix.Web.Css.Data.Declarations.Width;

public record max_content : Declaration<Keywords.max_content>
{
    public new Properties.Values.Width Property { get; init; }
    public new Properties.Width.Value<Keywords.max_content>? Value { get; init; }
}

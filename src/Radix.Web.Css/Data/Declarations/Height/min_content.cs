namespace Radix.Web.Css.Data.Declarations.Height;

public record min_content : Declaration<Keywords.min_content>
{
    public new Properties.Height.Value<Keywords.min_content>? Value { get; init; }
}

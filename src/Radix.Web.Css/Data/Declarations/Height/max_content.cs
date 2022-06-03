namespace Radix.Web.Css.Data.Declarations.Height;

public record max_content : Declaration<Keywords.max_content>
{
    public Properties.Height.Value<Keywords.max_content> Value { get; init; }
}

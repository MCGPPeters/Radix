namespace Radix.Web.Css.Data.Declarations.Width;

public record revert : Declaration<Keywords.revert>
{
    public Properties.Values.Width Property { get; init ; }
    public Properties.Width.Value<Keywords.revert> Value { get; init; }
}

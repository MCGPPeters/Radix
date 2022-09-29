namespace Radix.Web.Css.Data.Declarations.Width;

public record revert : Declaration<Keywords.revert>
{
    public new Properties.Values.Width Property { get; init ; }
    public new Properties.Width.Value<Keywords.revert>? Value { get; init; }
}

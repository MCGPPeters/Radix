namespace Radix.Web.Css.Data.Declarations.Height;

public record revert : Declaration<Keywords.revert>
{
    public new Properties.Height.Value<Keywords.revert>? Value { get; init; }
}

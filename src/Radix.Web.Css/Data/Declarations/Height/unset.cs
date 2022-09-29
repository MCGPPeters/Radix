namespace Radix.Web.Css.Data.Declarations.Height;

public record unset : Declaration<Keywords.unset>
{
    public new Properties.Height.Value<Keywords.unset>? Value { get; init; }
}

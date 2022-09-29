namespace Radix.Web.Css.Data.Declarations.Height;

public record initial : Declaration<Keywords.initial>
{
    public new Properties.Height.Value<Keywords.initial>? Value { get; init; }
}

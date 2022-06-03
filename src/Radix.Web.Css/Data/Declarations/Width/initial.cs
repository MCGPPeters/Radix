namespace Radix.Web.Css.Data.Declarations.Width;

public record initial : Declaration<Keywords.initial>
{
    public Properties.Width.Value<Keywords.initial> Value { get; init; }
}

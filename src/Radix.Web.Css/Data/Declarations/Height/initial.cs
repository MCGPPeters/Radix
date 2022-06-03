namespace Radix.Web.Css.Data.Declarations.Height;

public record initial : Declaration<Keywords.initial>
{
    public Properties.Height.Value<Keywords.initial> Value { get; init; }
}

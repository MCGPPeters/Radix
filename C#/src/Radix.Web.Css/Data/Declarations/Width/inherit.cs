namespace Radix.Web.Css.Data.Declarations.Width;

public record inherit : Declaration<Keywords.inherit>
{
    public Properties.Width.Value<Keywords.inherit> Value { get; init; }
}

namespace Radix.Web.Css.Data.Declarations.Width;

public partial struct inherit : Declaration<Keywords.inherit>
{
    public Properties.Values.Width Property { get; init; }
    public Properties.Width.Value<Keywords.inherit> Value { get; init; }
}

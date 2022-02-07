namespace Radix.Web.Css.Data.Declarations.Height;

public partial struct inherit : Declaration<Keywords.inherit>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Keywords.inherit> Value { get; init; }
}

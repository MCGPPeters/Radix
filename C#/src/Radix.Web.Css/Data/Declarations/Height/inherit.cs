namespace Radix.Web.Css.Data.Declarations.Height;

public partial record inherit : Declaration<Keywords.inherit>
{
    public Properties.Height.Value<Keywords.inherit> Value { get; init; }
}

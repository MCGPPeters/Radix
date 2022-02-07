namespace Radix.Web.Css.Data.Declarations.Width;

public partial struct inherit : Declaration<Global.inherit>
{
    public Properties.Values.Width Property { get; init; }
    public Properties.Width.Value<Global.inherit> Value { get; init; }
}

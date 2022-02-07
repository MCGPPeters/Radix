namespace Radix.Web.Css.Data.Declarations.Height;

public partial struct inherit : Declaration<Global.inherit>
{
    public Properties.Values.Height Property { get; init; }
    public Properties.Width.Value<Global.inherit> Value { get; init; }
}

namespace Radix.Web.Css.Data.Declarations.Width;

public partial struct max_content : Declaration<Keywords.max_content>
{
    public Properties.Values.Width Property { get; init; }
    public Properties.Width.Value<Keywords.max_content> Value { get; init; }
}

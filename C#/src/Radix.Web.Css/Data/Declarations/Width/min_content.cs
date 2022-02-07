namespace Radix.Web.Css.Data.Declarations.Width;

public partial struct min_content : Declaration<Keywords.min_content>
{
    public Properties.Values.Width Property { get; init; }
    public Properties.Width.Value<Keywords.min_content> Value { get; init; }
}

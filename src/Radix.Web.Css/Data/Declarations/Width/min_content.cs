namespace Radix.Web.Css.Data.Declarations.Width;

public partial record min_content : Declaration<Keywords.min_content>
{
    public new Properties.Values.Width Property { get; init; }
    public new Properties.Width.Value<Keywords.min_content>? Value { get; init; }
}

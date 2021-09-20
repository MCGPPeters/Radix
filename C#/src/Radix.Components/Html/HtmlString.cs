namespace Radix.Components.Html;

public record struct HtmlString(string Value) : Alias<HtmlString, string>, Node
{
    public static implicit operator string(HtmlString htmlString) => htmlString.Value;
    public static implicit operator HtmlString(string htmlString) => new(htmlString);
}

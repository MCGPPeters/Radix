namespace Radix.Components.Html;

public record HtmlString(string Value) : Alias<string>(Value), Node;

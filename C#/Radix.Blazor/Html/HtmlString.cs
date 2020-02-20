using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public struct HtmlString : Node, Value<string>
    {
        public string Value { get; set; }
    }
}

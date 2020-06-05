using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public class Text : Node, Value<string>
    {
        public Text(string text) => Value = text;

        public string Value { get; }
    }
}

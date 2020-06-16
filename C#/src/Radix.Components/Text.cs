using Radix.Components.Html;

namespace Radix.Components
{
    public class Text : Node, Value<string>
    {
        public Text(string text) => Value = text;

        public string Value { get; }
    }
}

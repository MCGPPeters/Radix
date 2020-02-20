using Radix.Blazor.Html;

namespace Radix.Blazor
{
    public readonly struct Text : Node, Value<string>
    {
        public Text(string text) : this()
        {
            Value = text;
        }

        public string Value { get; }
    }
}

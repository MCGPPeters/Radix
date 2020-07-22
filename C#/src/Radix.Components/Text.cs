using Radix.Components.Html;

namespace Radix.Components
{
    public record Text(string value) : Alias<string>(value), Node;
}

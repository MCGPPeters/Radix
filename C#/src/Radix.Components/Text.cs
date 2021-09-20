using Radix.Components.Html;

namespace Radix.Components;

public record Text(string value) : Alias<Text, string>, Node
{
    public static implicit operator Text(string value) => new(value);

    public static implicit operator string(Text value) => value;
}

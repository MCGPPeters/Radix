using Radix.Components.Html;
using Radix.Components.Nodes;

namespace Radix.Components;

public static class Prelude
{
    public static Node text(string text) => new Text(text);

    public static Concat concat(params Node[] nodes) => new(nodes);
}

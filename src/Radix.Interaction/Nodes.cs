using System.Runtime.CompilerServices;
using Radix.Interaction.Data;

namespace Radix.Interaction;

public static class Nodes
{
    public static Node text(string text, [CallerLineNumber] int nodeId = 0) =>
        (component, builder) => builder.AddContent(nodeId, text);

    public static Node concat(Node[] nodes, [CallerLineNumber] int nodeId = 0) =>
        (component, builder) =>
        {
            foreach (Node node in nodes)
                node(component, builder);
        };

    public static Node empty([CallerLineNumber] int nodeId = 0) => (_, __) => { };
}

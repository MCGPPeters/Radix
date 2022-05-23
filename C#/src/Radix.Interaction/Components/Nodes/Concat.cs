using System.Collections;
using Radix.Interaction.Data;

namespace Radix.Interaction.Components.Nodes;

public record Concat : Node, IEnumerable<Node>
{
    public Concat(NodeId nodeId, Node[] nodes) : base(nodeId) => Nodes = new List<Node>(nodes);

    public Node this[int index]
    {
        get => Nodes[index];
        set => Nodes.Insert(index, value);
    }

    public List<Node> Nodes { get; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Node> GetEnumerator() => Nodes.GetEnumerator();
}

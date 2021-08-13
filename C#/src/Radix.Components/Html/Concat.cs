using System.Collections;

namespace Radix.Components.Html;

public class Concat : Node, IEnumerable<Node>
{
    public Concat(Node[] nodes) => Nodes = new List<Node>(nodes);

    public Node this[int index]
    {
        get => Nodes[index];
        set => Nodes.Insert(index, value);
    }

    public List<Node> Nodes { get; }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Node> GetEnumerator() => Nodes.GetEnumerator();
}

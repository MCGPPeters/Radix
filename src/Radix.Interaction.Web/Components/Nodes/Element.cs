using Microsoft.AspNetCore.Components;
using Radix.Interaction.Data;

namespace Radix.Interaction.Web.Components.Nodes;

public record Element(NodeId NodeId, string Name, IEnumerable<Data.Attribute> Attributes, IEnumerable<Node> Children) :  Node(NodeId)
{
    public ElementReference ElementReference { get; set; }
}

public record Element<T> : Element
    where T : Literal<T>, Radix.Web.Html.Data.Names.Element
{
    public Element(NodeId NodeId, Data.Attribute[] attributes, Node[] children) : base(NodeId, T.Format(), attributes, children) { }

    public Element(NodeId NodeId, Data.Attribute[] attributes) : this(NodeId, attributes, Array.Empty<Node>())
    {
    }

    public Element(NodeId NodeId, Node[] nodes) : this(NodeId, Array.Empty<Data.Attribute>(), nodes)
    {
    }
}

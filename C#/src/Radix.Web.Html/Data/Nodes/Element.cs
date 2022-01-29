using Microsoft.AspNetCore.Components;
using Radix.Components;
using Radix.Web.Html.Data.Names;

namespace Radix.Web.Html.Data.Nodes;

public class Element : Node
{
    public Element(string name, IEnumerable<Attribute> attributes, IEnumerable<Node> children)
    {
        Name = name;
        Attributes = attributes;
        Children = children;
    }

    public string Name { get; init; }
    public IEnumerable<Attribute> Attributes { get; init;}
    public IEnumerable<Node> Children { get; init; }

    public ElementReference ElementReference { get; set; }
}

public class Element<T> : Element
    where T : Literal<T>, TagName
{
    public Element(IEnumerable<Attribute> attributes, IEnumerable<Node> children) : base(T.Format(), attributes, children) { }

    public Element(IEnumerable<Attribute> attributes) : this(attributes, Array.Empty<Node>())
    {
    }

    public Element(IEnumerable<Node> nodes) : this(Enumerable.Empty<Attribute>(), nodes)
    {
    }
}

using Microsoft.AspNetCore.Components;

namespace Radix.Components.Html;

public class Element : Node
{
    public Element(string name, IEnumerable<IAttribute> attributes, IEnumerable<Node> children)
    {
        Name = name;
        Attributes = attributes;
        Children = children;
    }

    public Element(string name, IEnumerable<IAttribute> attributes) : this(name, attributes, Array.Empty<Node>())
    {
    }

    public Element(Name name, IEnumerable<Node> nodes) : this(name, Enumerable.Empty<IAttribute>(), nodes)
    {
    }

    public ElementReference ElementReference { get; set; }
    public string Name { get; }
    public IEnumerable<IAttribute> Attributes { get; }
    public IEnumerable<Node> Children { get; }
}

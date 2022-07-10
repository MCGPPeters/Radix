using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Radix.Interaction.Components;
using Radix.Interaction.Components.Nodes;
using Radix.Interaction.Data;

namespace Radix.Interaction.Web.Components;

public static class Components
{
    public static component navLinkMatchAll => (nodeId, attributes, children) => navLink(NavLinkMatch.All)(nodeId, attributes, children);

    public static component navLinkMatchPrefix => (nodeId, attributes, children) => navLink(NavLinkMatch.Prefix)(nodeId, attributes, children);

    public static component navLink(NavLinkMatch navLinkMatch) => (nodeId, attributes, children) =>
        component<NavLink>(nodeId, attributes.Prepend(new ComponentAttribute(nodeId, "Match", navLinkMatch)), children);

    public static Component component<T>(NodeId nodeId, IEnumerable<Data.Attribute> attributes, params Node[] children)
        where T : IComponent => new(nodeId, typeof(T), attributes, children);

    public static Component component<T>(NodeId nodeId, params Data.Attribute[] attributes)
        where T : IComponent => new(nodeId, typeof(T), attributes);
}

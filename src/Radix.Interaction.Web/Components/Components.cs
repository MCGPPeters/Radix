using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Radix.Interaction.Components;
using Radix.Interaction.Components.Nodes;
using Radix.Interaction.Data;

namespace Radix.Interaction.Web.Components;

public static class Components
{
    public static component navLinkMatchAll => (attributes, children, nodeId) => navLink(NavLinkMatch.All)( attributes, children, nodeId);

    public static component navLinkMatchPrefix => (nodeId, attributes, children) => navLink(NavLinkMatch.Prefix)(nodeId, attributes, children);

    public static component navLink(NavLinkMatch navLinkMatch) => (attributes, children, nodeId) =>
        component<NavLink>(attributes.Prepend(new ComponentAttribute((NodeId)nodeId, "Match", navLinkMatch)), children);

    public static Component component<T>(IEnumerable<Data.Attribute> attributes, Node[] children, [CallerLineNumber] int nodeId = 0)
        where T : IComponent => new(typeof(T), attributes, children, nodeId);

    public static Component component<T>(Data.Attribute[] attributes, [CallerLineNumber] int nodeId = 0)
        where T : IComponent => new(typeof(T), attributes, nodeId);
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Radix.Components.Nodes;

namespace Radix.Components;

public static class Components
{
    public static component navLinkMatchAll => (attributes, children) => navLink(NavLinkMatch.All)(attributes, children);

    public static component navLinkMatchPrefix => (attributes, children) => navLink(NavLinkMatch.Prefix)(attributes, children);

    public static component navLink(NavLinkMatch navLinkMatch) => (attributes, children) =>
        component<NavLink>(attributes.Prepend(new ComponentAttribute("Match", navLinkMatch)), children);

    public static Nodes.Component component<T>(IEnumerable<Attribute> attributes, params Node[] children)
        where T : IComponent => new(typeof(T), attributes, children);

    public static Nodes.Component component<T>(params Attribute[] attributes)
        where T : IComponent => new(typeof(T), attributes);
}

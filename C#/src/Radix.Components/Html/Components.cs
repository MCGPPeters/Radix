using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Radix.Components.Html;

public static class Components
{

    public static component navLinkMatchAll => (attributes, children) => navLink(NavLinkMatch.All)(attributes, children);
    public static component navLinkMatchPrefix => (attributes, children) => navLink(NavLinkMatch.Prefix)(attributes, children);

    public static component navLink(NavLinkMatch navLinkMatch) => (attributes, children) =>
        component<NavLink>(attributes.Prepend(new ComponentAttribute("Match", navLinkMatch)), children);

    public static Component component<T>(IEnumerable<IAttribute> attributes, params Node[] children)
        where T : IComponent => new(typeof(T), attributes, children);
}

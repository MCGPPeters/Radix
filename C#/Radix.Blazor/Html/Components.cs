using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Radix.Blazor.Html
{
    public static class Components
    {

        public static component navLinkMatchAll => (attributes, children) => navLink(NavLinkMatch.All)(attributes, children);
        public static component navLinkMatchPrefix => (attributes, children) => navLink(NavLinkMatch.Prefix)(attributes, children);

        public static component navLink(NavLinkMatch navLinkMatch)
        {
            return (attributes, children) =>
            {

                attributes.Prepend(new ComponentAttribute("Match", navLinkMatch));

                return component<NavLink>(attributes, children);
            };
        }

        public static Component component<T>(IEnumerable<IAttribute> attributes, params Node[] children)
            where T : IComponent
        {
            return new Component(typeof(T), attributes, children);
        }
    }

}

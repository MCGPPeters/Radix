using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using static Radix.Interaction.Component;
using Radix.Interaction.Data;

namespace Radix.Interaction.Web;

public static class Components
{
    public static Data.Node component<T>(Data.Attribute[] attributes, Node[] children, [CallerLineNumber] int nodeId = 0) where T : IComponent => Create<T>(attributes, children, nodeId);

    public static Data.Node navLinkMatchAll(Data.Attribute[] attributes, Node[] children, [CallerLineNumber] int nodeId = 0) => navLink(NavLinkMatch.All, attributes, children, nodeId);

    public static Data.Node navLinkMatchPrefix(Data.Attribute[] attributes, Node[] children, [CallerLineNumber] int nodeId = 0) => navLink(NavLinkMatch.Prefix, attributes, children, nodeId);

    public static Data.Node navLink(NavLinkMatch navLinkMatch, Data.Attribute[] attributes, Node[] children, [CallerLineNumber] int nodeId = 0) =>
        Create<NavLink>(attributes.Prepend(Attribute.Create("Match", [navLinkMatch])).ToArray(), children);

}

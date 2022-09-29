using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using static Radix.Interaction.Web.Components.Components;

namespace Radix.Inventory.Shared;


public class NavMenu : Component<NavMenuModel, object>
{
    private bool collapseNavMenu = true;

    protected override Interaction.Update<NavMenuModel, object> Update => (model, _) => Task.FromResult(model);
    private string NavMenuCssClass => collapseNavMenu ? "collapse" : "";

    protected override View<NavMenuModel, object> View =>
        async (model, _)
            =>
            await Task.FromResult(concat
            (
                (NodeId)1,
                div
                (
                    (NodeId)2,
                    @class((NodeId)1, "top-row", "pl-4", "navbar", "navbar-dark"),
                    a
                    (
                        (NodeId)3,
                        new[]
                        {
                            @class((NodeId)2, "navbar-brand"),
                            href((NodeId)3, "")
                        },
                        text
                        (
                            (NodeId)4,
                            "Radix.Inventory"
                        )
                    ),
                    button
                    (
                        (NodeId)5,
                        new[]
                        {
                            @class((NodeId)4, "navbar-toggler"),
                            on.click((NodeId)5, _ => ToggleNavMenu())
                        },
                        span
                        (
                            (NodeId)6,
                            @class((NodeId)6, "navbar-toggler-icon")
                        )
                    )
                ),
                div
                (
                    (NodeId)7,
                    new[]
                    {
                        @class((NodeId)7, NavMenuCssClass),
                        on.click((NodeId)8, _ => ToggleNavMenu())
                    },
                    ul
                    (
                        (NodeId)8,
                        @class((NodeId)9, "nav", "flex-column"),
                        li
                        (
                            (NodeId)9,
                            @class((NodeId)10, "nav-item", "px-3"),
                            navLinkMatchAll
                            (
                                (NodeId)10,
                                new[]
                                {
                                    @class((NodeId)11, "nav-link"),
                                    href((NodeId)12, "")
                                },
                                span
                                (
                                    (NodeId)11,
                                    new[]
                                    {
                                        @class((NodeId)13, "oi", "oi-home"),
                                        attribute((NodeId)14, "aria-hidden", "true")
                                    }
                                ),
                                text
                                (
                                    (NodeId)12,
                                    "Home"
                                )
                            )
                        ),
                        li
                        (
                            (NodeId)13,
                            @class((NodeId)15, "nav-item", "px-3"),
                            navLinkMatchAll
                            (
                                (NodeId)14,
                                new[]
                                {
                                    @class((NodeId)15, "nav-link"),
                                    href((NodeId)16, "counter")
                                },
                                span
                                (
                                    (NodeId)15,
                                    new[]
                                    {
                                        @class((NodeId)17, "oi", "oi-plus"),
                                        attribute((NodeId)18, "aria-hidden", "true")
                                    }
                                ),
                                text
                                (
                                    (NodeId)16,
                                    "Counter"
                                )
                            )
                        ),
                        li
                        (
                            (NodeId)17,
                            @class((NodeId)19, "nav-item", "px-3"),
                            navLinkMatchAll
                            (
                                (NodeId)18,
                                new[]
                                {
                                    @class((NodeId)20, "nav-link"),
                                    href((NodeId)21, "fetchdata")
                                },
                                span
                                (
                                    (NodeId)19,
                                    new[]
                                    {
                                        @class((NodeId)22, "oi", "oi-list-rich"),
                                        attribute((NodeId)22, "aria-hidden", "true")
                                    }
                                ),
                                text
                                (
                                    (NodeId)20,
                                    "Fetch data"
                                )
                            )
                        )
                    )
                    
                )
            ));

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }

}

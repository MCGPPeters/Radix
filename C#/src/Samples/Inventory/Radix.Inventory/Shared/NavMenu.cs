using Radix.Components;
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

    protected override Interact<NavMenuModel, object> Interact =>
        async (model, _)
            =>
            concat
            (
                (NodeId)1,
                div
                (
                    (NodeId)2,
                    @class((AttributeId)1, "top-row", "pl-4", "navbar", "navbar-dark"),
                    a
                    (
                        (NodeId)3,
                        new[]
                        {
                            @class((AttributeId)2, "navbar-brand"),
                            href((AttributeId)3, "")
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
                            @class((AttributeId)4, "navbar-toggler"),
                            on.click((AttributeId)5, _ => ToggleNavMenu())
                        },
                        span
                        (
                            (NodeId)6,
                            @class((AttributeId)6, "navbar-toggler-icon")
                        )
                    )
                ),
                div
                (
                    (NodeId)7,
                    new[]
                    {
                        @class((AttributeId)7, NavMenuCssClass),
                        on.click((AttributeId)8, _ => ToggleNavMenu())
                    },
                    ul
                    (
                        (NodeId)8,
                        @class((AttributeId)9, "nav", "flex-column"),
                        li
                        (
                            (NodeId)9,
                            @class((AttributeId)10, "nav-item", "px-3"),
                            navLinkMatchAll
                            (
                                (NodeId)10,
                                new[]
                                {
                                    @class((AttributeId)11, "nav-link"),
                                    href((AttributeId)12, "")
                                },
                                span
                                (
                                    (NodeId)11,
                                    new[]
                                    {
                                        @class((AttributeId)13, "oi", "oi-home"),
                                        attribute((AttributeId)14, "aria-hidden", "true")
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
                            @class((AttributeId)15, "nav-item", "px-3"),
                            navLinkMatchAll
                            (
                                (NodeId)14,
                                new[]
                                {
                                    @class((AttributeId)15, "nav-link"),
                                    href((AttributeId)16, "counter")
                                },
                                span
                                (
                                    (NodeId)15,
                                    new[]
                                    {
                                        @class((AttributeId)17, "oi", "oi-plus"),
                                        attribute((AttributeId)18, "aria-hidden", "true")
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
                            @class((AttributeId)19, "nav-item", "px-3"),
                            navLinkMatchAll
                            (
                                (NodeId)18,
                                new[]
                                {
                                    @class((AttributeId)20, "nav-link"),
                                    href((AttributeId)21, "fetchdata")
                                },
                                span
                                (
                                    (NodeId)19,
                                    new[]
                                    {
                                        @class((AttributeId)22, "oi", "oi-list-rich"),
                                        attribute((AttributeId)22, "aria-hidden", "true")
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
            );

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }

}

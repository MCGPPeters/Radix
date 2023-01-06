using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using static System.Array;
using static Radix.Interaction.Web.Components.Components;
using Attribute = Radix.Interaction.Data.Attribute;

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
                new Node[]
                {
                    div
                    (
                        new Attribute[]
                        {
                            @class((NodeId)1, "top-row", "pl-4", "navbar", "navbar-dark"),
                        },
                        new Node[]
                            {
                        a
                        (
                            new Attribute[]
                            {
                                @class((NodeId)2, "navbar-brand"),
                                href((NodeId)3, "")
                            },
                            new[]
                            {
                                text
                                (
                                    "Radix.Inventory"
                                )
                            }
                        ),
                        button
                        (
                            new[]
                            {
                                @class((NodeId)4, "navbar-toggler"),
                                on.click((NodeId)5, _ => ToggleNavMenu())
                            },
                            new Node[]
                            {
                                span
                                (
                                    new Attribute[]
                                    {
                                        @class((NodeId)6, "navbar-toggler-icon")
                                    },
                                    Empty<Node>()
                                    
                                )
                            }
                            
                        )
                        }
                    ),
                
                    div
                    (
                        new Attribute[]
                        {
                            @class((NodeId)7, NavMenuCssClass),
                            on.click((NodeId)8, _ => ToggleNavMenu())
                        },
                        new Node[]
                        {
                            ul
                            (
                                new Attribute[]
                                {
                                    @class((NodeId)9, "nav", "flex-column")
                                },
                                new Node[]
                                {
                                    li
                                    (
                                        new Attribute[]
                                        {
                                            @class((NodeId)10, "nav-item", "px-3"),
                                        },
                                        new Node[]
                                        {
                                            navLinkMatchAll
                                            (
                                                new[]
                                                {
                                                    @class((NodeId)11, "nav-link"),
                                                    href((NodeId)12, "")
                                                },
                                                new Node[]
                                                {
                                                    span
                                                    (
                                                        new Attribute[]
                                                        {
                                                            @class((NodeId)13, "oi", "oi-home"),
                                                            attribute((NodeId)14, "aria-hidden", "true")
                                                        },
                                                        Empty<Node>()
                                                    ),
                                                    text
                                                    (
                                                        "Home"
                                                    )
                                                }
                                            
                                            )
                                        }
                                    
                                    ),
                                    li
                                    (
                                        new Attribute[]
                                        {
                                            @class((NodeId)15, "nav-item", "px-3")
                                        },
                                        new Node[]
                                        {
                                            navLinkMatchAll
                                            (
                                                new Attribute[]
                                                {
                                                    @class((NodeId)15, "nav-link"),
                                                    href((NodeId)16, "counter")
                                                },
                                                new Node[]
                                                {
                                                    span
                                                    (
                                                        new[]
                                                        {
                                                            @class((NodeId)17, "oi", "oi-plus"),
                                                            attribute((NodeId)18, "aria-hidden", "true")
                                                        },
                                                        Empty<Node>()
                                                    ),
                                                    text
                                                    (
                                                        "Counter"
                                                    )
                                                }
                                    
                                            )
                                        }
                            
                                    ),
                                    li
                                    (
                                        new Attribute[]
                                        {
                                            @class((NodeId)19, "nav-item", "px-3")
                                        },
                                        new Node[]
                                        {
                                            navLinkMatchAll
                                            (
                                                new[]
                                                {
                                                    @class((NodeId)20, "nav-link"),
                                                    href((NodeId)21, "fetchdata")
                                                },
                                                new Node[]
                                                {
                                                    span
                                                    (
                                                        new[]
                                                        {
                                                            @class((NodeId)22, "oi", "oi-list-rich"),
                                                            attribute((NodeId)22, "aria-hidden", "true")
                                                        },
                                                        Empty<Node>()
                                                    ),
                                                    text
                                                    (
                                                        "Fetch data"
                                                    )
                                                }
                                            )
                                        }
                                    )
                                }
                            )
                        }
                    )
                }
            )
        );

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }

}

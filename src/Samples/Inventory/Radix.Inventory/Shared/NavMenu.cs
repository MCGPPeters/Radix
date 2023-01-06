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
                            @class(new[]
                            {
                                "top-row", "pl-4", "navbar", "navbar-dark"
                            }),
                        },
                        new Node[]
                            {
                        a
                        (
                            new Attribute[]
                            {
                                @class(new[]
                                {
                                    "navbar-brand"
                                }),
                                href(new []{""})
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
                                @class( new[] { "navbar-toggler" }),
                                on.click(_ => ToggleNavMenu())
                            },
                            new Node[]
                            {
                                span
                                (
                                    new Attribute[]
                                    {
                                        @class(new[] { "navbar-toggler-icon" })
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
                            @class(new []{ NavMenuCssClass}),
                            on.click(_ => ToggleNavMenu())
                        },
                        new Node[]
                        {
                            ul
                            (
                                new Attribute[]
                                {
                                    @class(new[]
                                    {
                                        "nav", "flex-column"
                                    })
                                },
                                new Node[]
                                {
                                    li
                                    (
                                        new Attribute[]
                                        {
                                            @class(new []{"nav-item", "px-3"}),
                                        },
                                        new Node[]
                                        {
                                            navLinkMatchAll
                                            (
                                                new Attribute[]
                                                {
                                                    @class(new[]
                                                    {
                                                        "nav-link"
                                                    }),
                                                    href(new []
                                                    {
                                                        ""
                                                    })
                                                },
                                                new Node[]
                                                {
                                                    span
                                                    (
                                                        new Attribute[]
                                                        {
                                                            @class(new[]
                                                            {
                                                                "oi", "oi-home"
                                                            }),
                                                            attribute("aria-hidden", new[]
                                                            {
                                                                "true"
                                                            } )
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
                                            @class(new []
                                            {
                                                "nav-item", "px-3"
                                            })
                                        },
                                        new Node[]
                                        {
                                            navLinkMatchAll
                                            (
                                                new Attribute[]
                                                {
                                                    @class(new[]
                                                    {
                                                        "nav-link"
                                                    }),
                                                    href( new[] { "counter" })
                                                },
                                                new Node[]
                                                {
                                                    span
                                                    (
                                                        new[]
                                                        {
                                                            @class( new[] { "oi" , "oi-plus"}),
                                                            attribute("aria-hidden", new[] { "true" })
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
                                            @class(new[] { "nav-item", "px-3" })
                                        },
                                        new Node[]
                                        {
                                            navLinkMatchAll
                                            (
                                                new[]
                                                {
                                                    @class(new[] { "nav-link" }),
                                                    href(new[] { "fetchdata" })
                                                },
                                                new Node[]
                                                {
                                                    span
                                                    (
                                                        new[]
                                                        {
                                                            @class(new[] { "oi", "oi-list-rich" }),
                                                            attribute("aria-hidden", new[] { "true" })
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

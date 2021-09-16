using Radix.Components;
using Radix.Components.Html;

namespace Radix.Inventory.Shared;


public class NavMenu : Component<NavMenuViewModel>
{
    private bool collapseNavMenu = true;

    private string NavMenuCssClass => collapseNavMenu ? "collapse" : "";

    protected override Node View(NavMenuViewModel currentViewModel)
        =>
        concat
        (
            div
            (
                @class("top-row pl-4 navbar navbar-dark"),
                a
                (
                    new[]
                    {
                        @class("navbar-brand"),
                        href("")
                    },
                    text
                    (
                        "Radix.Inventory"
                    )
                ),
                button
                (
                    new[]
                    {
                        @class("navbar-toggler"),
                        on.click(_ => ToggleNavMenu())
                    },
                    span
                    (
                        @class("navbar-toggler-icon")
                    )
                )
            ),
            div
            (
                new[]
                {
                    @class(NavMenuCssClass),
                    on.click(_ => ToggleNavMenu())
                },
                ul
                (
                    @class("nav flex-column"),
                    li
                    (
                        @class("nav-item px-3"),
                        navLinkMatchAll
                        (
                            new[]
                            {
                                @class("nav-link"),
                                href("")
                            },
                            span
                            (
                                new[]
                                {
                                    @class("oi oi-home"),
                                    attribute("aria-hidden", "true")
                                }
                            ),
                            text
                            (
                                "Home"
                            )
                        )
                    ),
                    li
                    (
                        @class("nav-item px-3"),
                        navLinkMatchAll
                        (
                            new[]
                            {
                                @class("nav-link"),
                                href("counter")
                            },
                            span
                            (
                                new[]
                                {
                                    @class("oi oi-plus"),
                                    attribute("aria-hidden", "true")
                                }
                            ),
                            text
                            (
                                "Counter"
                            )
                        )
                    ),
                    li
                    (
                        @class("nav-item px-3"),
                        navLinkMatchAll
                        (
                            new[]
                            {
                                @class("nav-link"),
                                href("fetchdata")
                            },
                            span
                            (
                                new[]
                                {
                                    @class("oi oi-list-rich"),
                                    attribute("aria-hidden", "true")
                                }
                            ),
                            text
                            (
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

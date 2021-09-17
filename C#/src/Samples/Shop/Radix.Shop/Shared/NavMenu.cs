using Radix.Components;
using Radix.Components.Html;

namespace Radix.Shop.Shared
{
    public class NavMenu : Component<NavMenuViewModel>
    {
        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        protected override Node View(NavMenuViewModel currentViewModel) =>
            concat
            (
                div
                (
                    @class("top-row ps-3 navbar navbar-dark"),
                    div
                    (
                        @class("container-fluid"),
                        a
                        (
                            new[]
                            {
                                @class("navbar-brand"),
                                href("")
                            },
                            text
                            (
                                "Radix.Shop"
                            )
                        ),
                        button
                        (
                            new[]
                            {
                                @class("navbar-toggler"),
                                Attributes.title("Navigation menu"),
                                on.click(_ => ToggleNavMenu())
                            },
                            span
                            (
                                @class("navbar-toggler-icon")
                            )
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
                    nav
                    (
                        @class("flex-column"),
                        div
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
                        )
                    )
                )
            );
    }
}

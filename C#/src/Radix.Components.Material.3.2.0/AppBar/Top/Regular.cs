using Radix.Components.Html;
using Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

namespace Radix.Components.Material._3._2._0.AppBar.Top;

public class Regular : Component<RegularViewModel>
{
    protected override Node View(RegularViewModel currentViewModel)
    {
        return
            header
            (
                new[]
                {
                        @class("mdc-top-app-bar"),
                        id(currentViewModel.Id ?? "")
                },
                div
                (
                    new[]
                    {
                            @class("mdc-top-app-bar__row")
                    },
                    section
                    (
                        new[]
                        {
                                @class("mdc-top-app-bar__section mdc-top-app-bar__section--align-start")
                        },
                        currentViewModel.NavigationButton is not null
                            ? currentViewModel.NavigationButton switch
                            {
                                Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons.Back _ => component<Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons.Back>(),
                                Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons.Up _ => component<Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons.Up>(),
                                Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons.Menu _ => component<Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons.Menu>(),
                                _ => new Empty()
                            }
                            :
                                new Empty(),
                        span
                        (
                            new[]
                            {
                                    @class("mdc-top-app-bar__title")
                            },
                            text
                            (
                                currentViewModel.PageTitle ?? ""
                            )
                        )
                    ),
                    section
                    (
                        new[]
                        {
                                @class("mdc-top-app-bar__section mdc-top-app-bar__section--align-end"),
                                attribute("role", "toolbar")
                        },
                        concat
                        (
                            currentViewModel.ActionButtons.Select(button =>
                                button switch
                                {
                                    Favorite _ => component<Favorite>(),
                                    Search _ => component<Search>(),
                                    Options _ => component<Options>(),
                                    ShoppingCart _ => component<ShoppingCart>(),
                                    _ => throw new NotImplementedException("Unknown action button type added to the App Bar")
                                }).ToArray()
                        )
                    )
                ),
                script
                (
                    text
                    (
                        "mdc.topAppBar.MDCTopAppBar.attachTo(document.querySelector('.mdc-top-app-bar'));"
                    )
                )
            );
    }
}

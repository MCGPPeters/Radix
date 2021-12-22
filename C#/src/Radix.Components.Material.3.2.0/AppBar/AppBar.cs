using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radix.Components.Html;

namespace Radix.Components.Material._3._2._0.AppBar;

public class Regular : Component<AppBarViewModel>
{
    protected override Node View(AppBarViewModel currentViewModel)
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
                                    Navigation.Buttons.Back _ => component<Navigation.Buttons.Back>(),
                                    Navigation.Buttons.Up _ => component<Navigation.Buttons.Up>(),
                                    Navigation.Buttons.Menu _ => component<Navigation.Buttons.Menu>(),
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
                                    Action.Buttons.Favorite _ => component<Action.Buttons.Favorite>(),
                                    Action.Buttons.Search _ => component<Action.Buttons.Search>(),
                                    Action.Buttons.Options _ => component<Action.Buttons.Options>(),
                                    _ => throw new NotImplementedException("Unknown action button type added to the App Bar")
                                }).ToArray()
                        ),
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
                    )
                )
            );
    }
}

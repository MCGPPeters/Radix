using Radix.Components.Html;
using Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;
using Radix.Data;

namespace Radix.Components.Material._3._2._0.AppBar.Top;

// todo : add tooltips

public class Regular : Component<RegularViewModel>
{
    private const string AppBarCssClassName = "mdc-top-app-bar";

    private const string SearchFormCssClassName = $"{AppBarCssClassName}-search-form";

    private const string SearchBackButtonCssClassName = $"{AppBarCssClassName}-search-back-button";

    private const string SearchSearchInputCssClassName = $"{AppBarCssClassName}-search-input";

    protected override Node View(RegularViewModel currentViewModel)
    {
        var hasSearch = currentViewModel.ActionButtons.Any(button => button is Search);
               
        return
            header
            (
                new[]
                {
                        @class($"{AppBarCssClassName} show"),
                        id(currentViewModel.Id ?? "")
                },
                script
                (
                    text
                    (
                        $@"
                            function showSearchBar() {{ document.querySelector('.mdc-top-app-bar').classList.add('search-show'); document.querySelector('.{SearchSearchInputCssClassName}').focus();  }}
                            function hideSearchBar() {{ document.querySelector('.mdc-top-app-bar').classList.remove('search-show') }}                            
                        "
                    )
                ),
                Styles(),
                div
                (
                    new[]
                    {
                            @class("mdc-top-app-bar__row")
                    },
                    hasSearch
                        ? SearchBar()
                        : new Empty()
                    ,
                    section
                    (
                        new[]
                        {
                                @class("mdc-top-app-bar__section mdc-top-app-bar__section--align-start")
                        },
                        currentViewModel.NavigationButton is not null
                            ? new Radix.Components.Nodes.Component(currentViewModel.NavigationButton.GetType(), Enumerable.Empty<Attribute>())
                            : new Empty(),
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
                                new Nodes.Component(button.GetType(), Enumerable.Empty<Attribute>())).ToArray()
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

    private Node SearchBar() =>
        form
        (
            @class
            (
                $"{SearchFormCssClassName} form"
            ),
            button
            (
                new[]
                {
                    type("button"),
                    @class($"{SearchBackButtonCssClassName} material-icons"),
                    aria_label("Exit search results"),
                    on.click(async _ => await JSRuntime.InvokeAsync<object>("hideSearchBar", Array.Empty<object>()))
                },
                text
                (
                    "arrow_back"
                )                
            ),
            input
            (
                new[]
                {
                    type("text"),
                    @class(SearchSearchInputCssClassName),
                    placeholder("Search"),
                    aria_label("Type what you want to search and press enter"),
                    autocomplete("off"),
                }
            )
        );

    private Node Styles() =>
        style
        (
            // todo : figure out media query in style element for search box font-size
            text
            (
                $@"
                    header.{AppBarCssClassName} .{SearchBackButtonCssClassName} {{
                        width: 72px;
                        border: 0;
                        background-color: white;
                        cursor: pointer;
                    }}

                    header.{AppBarCssClassName}.search-show .{SearchBackButtonCssClassName} {{
                        display: block;
                    }}

                    header.{AppBarCssClassName} .{SearchFormCssClassName} {{
                        background: #fff;
                        display: flex;
                        transition: opacity 100ms 100ms cubic-bezier(0.4, 0, 0.2, 1), visibility 100ms;
                        opacity: 0;
                        pointer-events: none;
                        visibility: hidden;
                        z-index: 4;
                        position: absolute;
                        top: 0;
                        right: 0;
                        bottom: 0;
                        left: 0;
                    }}

                    header.{AppBarCssClassName}.show {{
                        transform: translateY(0);
                        transition: transform 300ms 300ms cubic-bezier(0.4, 0, 0.2, 1), opacity 0ms 300ms, background-color 150ms 0ms cubic-bezier(0.4, 0, 0.2, 1);
                        opacity: 1;
                        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.5);
                    }}

                    header.{AppBarCssClassName}.search-show {{
                        transform: translateY(0);
                        opacity: 1;
                    }}

                    header.{AppBarCssClassName}.search-show .form {{
                        transition: opacity 100ms 0ms cubic-bezier(0.4, 0, 0.2, 1);
                        opacity: 1;
                        pointer-events: all;
                        visibility: visible;
                    }}

                    @media screen and (min-width: 921px) {{
                        .{SearchSearchInputCssClassName} {{
                            font-size: 1.5rem;
                        }}
                    }}

                    @media screen and (min-width: 521px) {{
                        .{SearchSearchInputCssClassName} {{
                            font-size: 1.375rem;
                        }}
                    }}

                    header.{AppBarCssClassName} .{SearchSearchInputCssClassName} {{
                            -moz-osx-font-smoothing: grayscale;
                            -webkit-font-smoothing: antialiased;
                            font-family: Roboto, sans-serif;
                            font-family: var(--mdc-typography-headline3-font-family, var(--mdc-typography-font-family, Roboto, sans-serif));
                            font-size: 1.375rem;
                            font-size: var(--mdc-typography-headline3-font-size, 1.375rem);
                            line-height: 1.2;
                            line-height: var(--mdc-typography-headline3-line-height, 1.2);
                            font-weight: 400;
                            font-weight: var(--mdc-typography-headline3-font-weight, 400);
                            letter-spacing: normal;
                            letter-spacing: var(--mdc-typography-headline3-letter-spacing, normal);
                            text-decoration: inherit;
                            -webkit-text-decoration: var(--mdc-typography-headline3-text-decoration, inherit);
                            text-decoration: var(--mdc-typography-headline3-text-decoration, inherit);
                            text-transform: inherit;
                            text-transform: var(--mdc-typography-headline3-text-transform, inherit);
                            position: relative;
                            top: 0px;
                            flex: 1;
                            height: 65px;
                            margin: 0;
                            padding: 0 0 40px;
                            transition: top 150ms 0ms cubic-bezier(0.4, 0, 0.2, 1);
                            border: 0;
                            outline: 0;
                            background: 0 0;
                            -webkit-appearance: none;
                            -moz-appearance: none;
                            appearance: none;
                    }}
                "
            )
        );
}

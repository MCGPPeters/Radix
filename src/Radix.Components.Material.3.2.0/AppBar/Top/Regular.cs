﻿

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;
using Microsoft.JSInterop;

using Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

using Radix.Interaction;

using Radix.Interaction.Web;


using Attribute = Radix.Interaction.Data.Attribute;


namespace Radix.Components.Material._3._2._0.AppBar.Top;

// todo : add tooltips

public class Regular : Component<RegularModel, RegularCommand>
{
    private const string AppBarCssClassName = "mdc-top-app-bar";

    private const string SearchFormCssClassName = $"{AppBarCssClassName}-search-form";

    private const string SearchBackButtonCssClassName = $"{AppBarCssClassName}-search-back-button";

    private const string SearchSearchInputCssClassName = $"{AppBarCssClassName}-search-input";

    [Inject] public IJSRuntime JSRuntime { get; set; } = null!;

    [Parameter] public EventCallback<string> OnSearchTermEntered { get; set; }



    public override async ValueTask<RegularModel> Update(RegularModel model, RegularCommand command) => model;
    public override Node[] View(RegularModel model, Func<RegularCommand, Task> dispatch)
    {
        var hasSearch = model.ActionButtons.Any(button => button is Search);

        return
            header
            (
                [
                    @class(new []{AppBarCssClassName, "show"}),
                    id(new []{model.Id})
                ],
                [
                        script
                        (
                            [],
                            [
                                new HtmlString
                                (
                                    $$"""
                                        function showSearchBar() { document.querySelector('.mdc-top-app-bar').classList.add('search-show'); document.querySelector('.{{SearchSearchInputCssClassName}}').focus(); alert();  }
                                        function hideSearchBar() { document.querySelector('.mdc-top-app-bar').classList.remove('search-show') }                           
                                    """
                                )
                            ]
                        ),
                        Styles(), div
                        (
                            [@class(["mdc-top-app-bar__row"])],

                            [
                                hasSearch
                                    ? SearchBar(model, dispatch)
                                    : [],
                                section
                                (
                                    new Attribute[]
                                    {
                                        @class(new []{"mdc-top-app-bar__section",
                                            "mdc-top-app-bar__section--align-start"})
                                    },
                                    new Node[]
                                    {
                                        model.NavigationButton is not null
                                            ? component<Navigation.Button>([], [])
                                            : new Empty(),
                                        span
                                        (
                                            new[] {@class(new[] { "mdc-top-app-bar__title" })},
                                            new[]
                                            {
                                                text
                                                (
                                                    model.PageTitle ?? ""
                                                )
                                            }
                                        )
                                    }
                                ),
                                section
                                (
                                    new[]
                                    {
                                        @class(new[] { "mdc-top-app-bar__section mdc-top-app-bar__section--align-end" }),
                                        attribute("role", new[] { "toolbar" })
                                    },
                                    concat
                                    (
                                        model.ActionButtons.Select(button => new Component(button.GetType(),
                                            Enumerable.Empty<Interaction.Data.Attribute<object>>())).ToArray()
                                    ).Nodes.ToArray()
                                ),
                                script
                                (

                                    Array.Empty<Attribute>(),
                                    new Node[]
                                    {
                                        new HtmlString
                                        (
                                            "mdc.topAppBar.MDCTopAppBar.attachTo(document.querySelector('.mdc-top-app-bar'));"
                                        )
                                    }
                                )
                            ])
                ]);
    }
private Node SearchBar(RegularModel model, Func<RegularCommand, Task> dispatch) =>
        form
        (
            new Attribute []
            {
                @class
                (
                    new []{$"{SearchFormCssClassName} form"}
                )
            },
            new Node[]
            {
                button
                (
                    new[]
                    {
                        type(new[] { "button" }),
                        @class(new[] { $"{SearchBackButtonCssClassName} material-icons" }),
                        aria_label(new[] { "Exit search results" }),
                        on.click(async _ => await JSRuntime.InvokeAsync<object>("hideSearchBar", []))
                    },
                    new []
                    {
                        text
                        (
                            "arrow_back"
                        )
                    }
                ),
                input
                (
                    new[]
                    {
                        type(new[] { "text" }),
                        @class(new[] { SearchSearchInputCssClassName }),
                        placeholder(new[] { "Search" }),
                        aria_label(new[] { "Type what you want to search and press enter" }),
                        autocomplete(new[] { "off" }),
                        bind.input(model.SearchTerm, searchTerm => model.SearchTerm = searchTerm),
                        on.keydown(async args =>
                        {
                            if (args.Key == "Enter")
                            {
                                await OnSearchTermEntered.InvokeAsync(model.SearchTerm);
                                StateHasChanged();
                            }
                        })
                    },
                    Array.Empty<Node>()
                    
                )
            }
        );

    private Node Styles() =>
        style
        (
            Array.Empty<Attribute>(),
            new[]
            {
                // todo : figure out media query in style element for search box font-size
                new HtmlString
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
            }
        );
}

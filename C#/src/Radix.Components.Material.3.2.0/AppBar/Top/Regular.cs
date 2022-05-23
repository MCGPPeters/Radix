
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;
using Radix.Data;
using Radix.Interaction;
using Radix.Interaction.Components;
using Radix.Interaction.Components.Nodes;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
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

    protected override Interact<RegularModel, RegularCommand> Interact =>
            async (model, dispatch) =>
            {
                var hasSearch = model.ActionButtons.Any(button => button is Search);

                return
                    header
                    (
                        (NodeId)1,
                        new[]
                        {
                            @class((AttributeId)1, AppBarCssClassName, "show"),
                            id((AttributeId)2,model.Id ?? "")
                        },
                        script
                        (
                            (NodeId)2,
                            text
                            (
                                (NodeId)3,
                                $@"
                                    function showSearchBar() {{ document.querySelector('.mdc-top-app-bar').classList.add('search-show'); document.querySelector('.{SearchSearchInputCssClassName}').focus();  }}
                                    function hideSearchBar() {{ document.querySelector('.mdc-top-app-bar').classList.remove('search-show') }}                            
                                "
                            )
                        ),
                        Styles(),
                        div
                        (
                            (NodeId)4,
                            new[]
                            {
                                @class((AttributeId)3, "mdc-top-app-bar__row")
                            },
                            hasSearch
                                ? SearchBar(model)
                                : new Empty((NodeId)30)
                            ,
                            section
                            (
                                (NodeId)5,
                                new[]
                                {
                                    @class((AttributeId)4, "mdc-top-app-bar__section", "mdc-top-app-bar__section--align-start")
                                },
                                model.NavigationButton is not null
                                    ? new Interaction.Components.Nodes.Component((NodeId)20, model.NavigationButton.GetType(), Enumerable.Empty<Interaction.Data.Attribute<object>>())
                                    : new Empty((NodeId)20),
                                span
                                (
                                    (NodeId)6,
                                    new[]
                                    {
                                        @class((AttributeId)5, "mdc-top-app-bar__title")
                                    },
                                    text
                                    (
                                        (NodeId)7,
                                        model.PageTitle ?? ""
                                    )
                                )
                            ),
                            section
                            (
                                (NodeId)8,
                                new[]
                                {
                                    @class((AttributeId)6,"mdc-top-app-bar__section mdc-top-app-bar__section--align-end"),
                                    attribute((AttributeId)7, "role", "toolbar")
                                },
                                concat
                                (
                                    (NodeId)9,
                                    model.ActionButtons.Select(button =>
                                        new Component((NodeId)20, button.GetType(), Enumerable.Empty<Interaction.Data.Attribute<object>>())).ToArray()
                                )
                            )
                        ),
                        script
                        (
                            (NodeId)10,
                            text
                            (
                                (NodeId)11,
                                "mdc.topAppBar.MDCTopAppBar.attachTo(document.querySelector('.mdc-top-app-bar'));"
                            )
                        )
                    );
            };

    protected override Interaction.Update<RegularModel, RegularCommand> Update => throw new NotImplementedException();

    private Node SearchBar(RegularModel model) =>
        form
        (
            (NodeId)11,
            @class
            (
                (AttributeId)1,
                $"{SearchFormCssClassName} form"
            ),
            button
            (
                (NodeId)12,
                new[]
                {
                    type((AttributeId)2, "button"),
                    @class((AttributeId)3, $"{SearchBackButtonCssClassName} material-icons"),
                    aria_label((AttributeId)4, "Exit search results"),
                    on.click((AttributeId)5, async _ => await JSRuntime.InvokeAsync<object>("hideSearchBar", Array.Empty<object>()))
                },
                text
                (
                    (NodeId)13,
                    "arrow_back"
                )
            ),
            input
            (
                (NodeId)14,
                new[]
                {
                    type((AttributeId)6, "text"),
                    @class((AttributeId)7, SearchSearchInputCssClassName),
                    placeholder((AttributeId)8, "Search"),
                    aria_label((AttributeId)9, "Type what you want to search and press enter"),
                    autocomplete((AttributeId)10, "off"),
                    bind.input((AttributeId)11, model.SearchTerm, searchTerm => model.SearchTerm = searchTerm),
                    on.keydown((AttributeId)12, async args =>
                    {
                        if (args.Key == "Enter")
                        {
                            await model.Search(model.SearchTerm);
                            StateHasChanged();
                        }
                    })
                }
            )
        );

    private Node Styles() =>
        style
        (
            (NodeId)15,
            // todo : figure out media query in style element for search box font-size
            text
            (
                (NodeId)16,
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

using Radix.Components;
using Radix.Components.Html;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;
using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace Radix.Shop.Catalog.Interface.Logic.Components
{
    [Route("/catalog/search")]
    public class Search : Component<SearchViewModel>
    {
        private Element? _searchInput;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_searchInput is not null && firstRender)
                await _searchInput.ElementReference.FocusAsync();
        }

        protected override Node View(SearchViewModel currentViewModel)
        {
            _searchInput =
                input
                (
                    @class("form-control"),
                    placeholder("Search for products"),
                    autocomplete("off"),
                    autocapitalize("off"),
                    attribute("aria-labelledby", "search-label"),
                    type("text"),
                    spellcheck("false"),
                    attribute("role", "textbox"),

                    bind.input(currentViewModel.SearchTerm, searchTerm => currentViewModel.SearchTerm = searchTerm),
                    on.keydown(async args =>
                    {
                        if (args.Key == "Enter")
                        {
                            await currentViewModel.ExecuteSearch();
                            StateHasChanged();
                        }
                    })
                );


            return concat
            (
                header
                (
                    @class("bg-light shadow-sm fixed-top data-fixed-element"),
                    div
                    (
                        @class("navbar navbar-expand-lg navbar-light shadow"),
                        div
                        (
                            @class("container-fluid"),
                            a
                            (
                                new[]
                                {
                                    @class("navbar-brand"),
                                    href("/")
                                },
                                img
                                (
                                    src("https://via.placeholder.com/142x34/")
                                )
                            ),

                            div
                            (
                                @class("input-group"),
                                _searchInput,
                                button
                                (
                                    new[]
                                    {
                                        @class("btn btn-large btn-primary"),
                                        type("button"),
                                        on.click(async _ =>
                                        {
                                            await currentViewModel.ExecuteSearch();
                                        StateHasChanged();
                                        })
                                    },
                                    i
                                    (
                                        @class("bi bi-search")
                                    )
                                )
                            )
                        )
                    )
                ),
                main
                (
                    new[]
                    {
                        @class("pt-5")
                    },
                    section
                    (
                        @class("ps-lg-4 pe-lg-3 pt-5"),
                        div
                        (
                            @class("row row-cols-2 row-cols-md-4 row-cols-lg-6 g-0 mx-n2"),
                            currentViewModel.Products.Select
                            (
                                product =>
                                {
                                    return
                                    div
                                    (
                                        @class("col g-4"),
                                        div
                                        (
                                            new[]
                                            {
                                                @class("card shadow-sm"),
                                                id("product-card"),
                                                attribute("style", "border:0; transition: all .15s ease-in-out"),

                                            },
                                            style
                                            (
                                                text
                                                (
                                                    "#product-card:hover #product-title { text-overflow: initial; overflow: initial; white-space: initial;}" +
                                                    "#product-card:hover { transform: scale(1.05); box-shadow: 0 .5rem 1rem rgba(0,0,0,.15)!important;"
                                                )
                                            ),
                                            img
                                            (
                                                new[]
                                                {
                                                    @class("card-img-top d-block overflow-hidden p-3"),
                                                    src(product.ImageSource)
                                                }

                                            ),
                                            div
                                            (
                                                @class("card-body py-2"),
                                                div
                                                (
                                                    @class("d-flex align-items-center justify-content-between"),
                                                    div
                                                    (
                                                        p
                                                        (
                                                            @class("text-accent mb-0"),
                                                            text
                                                            (
                                                                product.PriceUnits + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator
                                                            ),
                                                            sup
                                                            (
                                                                text
                                                                (
                                                                    product.PriceFraction
                                                                )
                                                            )
                                                        ),
                                                        p
                                                        (
                                                            attribute("style", "font-size: .6em"),
                                                            text
                                                            (
                                                                $"{product.UnitSize} {product.UnitOfMeasure}"
                                                            )
                                                        )
                                                    ),
                                                    
                                                        currentViewModel.GetMerchentLogo(product.MerchantName)
                                                    ,
                                                    div
                                                    (
                                                        p
                                                        (
                                                            @class("text-accent mb-0"),
                                                            text
                                                            (
                                                                product.PricePerUnitPriceUnits + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator
                                                            ),
                                                            sup
                                                            (
                                                                text
                                                                (
                                                                    product.PricePerUnitPriceFraction
                                                                )
                                                            )
                                                        ),
                                                        p
                                                        (
                                                            attribute("style", "font-size: .6em"),
                                                            text
                                                            (
                                                                $"/ {product.UnitOfMeasure}"
                                                            )
                                                        )
                                                    )
                                                ),
                                                h6
                                                (
                                                    new[]
                                                    {
                                                        @class("text-truncate card-title"),
                                                        id("product-title")
                                                    },
                                                    text
                                                    (
                                                        product.Title
                                                    )
                                                )
                                            )
                                        )
                                    );
                                }
                            ).ToArray()
                        )
                    )
                )
            );
        }
    }
}

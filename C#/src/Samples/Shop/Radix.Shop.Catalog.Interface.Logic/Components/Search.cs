using Radix.Components;
using Radix.Components.Html;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;
using Radix.Shop.Catalog.Domain;
using Microsoft.AspNetCore.Components;
using Radix.Option;

namespace Radix.Shop.Catalog.Interface.Logic.Components
{
    [Route("/catalog/search")]
    public class Search : Component<SearchViewModel>
    {
        private Element? _searchInput;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_searchInput is not null && firstRender) _searchInput.ElementReference.FocusAsync();
        }

        protected override Node View(SearchViewModel currentViewModel)
        {
            _searchInput =
                input
                (
                    @class("form-control"),
                    placeholder("What are you looking for?"),
                    autocomplete("off"),
                    autocapitalize("off"),
                    attribute("aria-label", "search"),
                    type("text"),
                    spellcheck("false"),
                    attribute("role", "textbox"),

                    bind.input(currentViewModel.SearchTerm, searchTerm => currentViewModel.SearchTerm = searchTerm)
                );

            return concat
            (
                form
                (
                    attribute("onsubmit", "return false"),
                    div
                    (
                        @class("input-group"),
                        _searchInput,
                        button
                        (
                            new[]
                            {
                                @class("btn btn-primary"),
                                type("submit"),

                                on.click(async args =>
                                {
                                    currentViewModel.Products = new List<Product>();

                                    await foreach(var product in currentViewModel.Search((SearchTerm)currentViewModel.SearchTerm))
                                    {
                                        currentViewModel.Products.Add(product);
                                        StateHasChanged();
                                    };

                                    await currentViewModel.CrawlingMessageChannel.Writer.WriteAsync((SearchTerm)currentViewModel.SearchTerm);
                                })
                            },
                            text
                            (
                                "Search"
                            )
                        )
                    )
                ),
                div
                (
                    @class("row row-cols-auto g-1 "),
                    currentViewModel.Products.Select(product =>
                        div
                        (
                            @class("col-md-3"),
                            div
                            (
                                new[]
                                {
                                    @class("card p-3")
                                },
                                div
                                (
                                    @class("text-center"),
                                    img
                                    (
                                        new[]
                                        {
                                            @class("card-img-top"),
                                            src(product.ImageSource),
                                            width("200")
                                        }
                                    )
                                ),
                                div
                                (
                                    @class("card-body"),
                                    h6
                                    (
                                        @class("card-title"),
                                        text
                                        (
                                            product.Title
                                        )
                                    ),
                                    p
                                    (
                                        @class("card-text"),
                                        span
                                        (
                                            @class("font-weight-bold d-block"),
                                            text
                                            (
                                                product.PriceUnits.ToString()
                                            ),
                                            sup
                                            (
                                                text
                                                (
                                                    product.PriceFraction.ToString() + " "
                                                )
                                            )
                                        ),
                                        text
                                        (
                                            product.UnitOfMeasure
                                        )
                                        
                                    )
                                    //p
                                    //(
                                    //    @class("card-text"),
                                    //    small
                                    //    (
                                    //        @class("text-muted"),
                                    //        text
                                    //        (
                                    //            product.Price.Units
                                    //        ),
                                    //        sup
                                    //        (
                                    //            text
                                    //            (
                                    //                $",{product.Price.Fraction}"
                                    //            )
                                    //        ),
                                    //        text
                                    //        (
                                    //            product.UnitOfMeasure
                                    //        )
                                    //    )
                                    //),
                                    
                                )
                            )
                        )
                    ).ToArray()
                )
            );
        }
    }
}

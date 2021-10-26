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
                    @class("form-control w-100"),
                    placeholder("Search for products"),
                    autocomplete("off"),
                    autocapitalize("off"),
                    attribute("aria-labelledby", "search-label"),
                    type("text"),
                    spellcheck("false"),
                    attribute("role", "textbox"),
                    attribute("style", "padding-left: 2.5rem; width: 100% !important"),

                    bind.input(currentViewModel.SearchTerm, searchTerm => currentViewModel.SearchTerm = searchTerm),
                    on.keydown(async args =>
                    {
                        if (args.Key == "Enter")
                        {
                            currentViewModel.Products = new List<Product>();

                            await foreach (var product in currentViewModel.Search((SearchTerm)currentViewModel.SearchTerm))
                            {
                                currentViewModel.Products.Add(product);
                                StateHasChanged();
                            };

                            await currentViewModel.CrawlingMessageChannel.Writer.WriteAsync((SearchTerm)currentViewModel.SearchTerm);
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
                        @class("navbar navbar-expand-lg navbar-light"),
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
                                i
                                (
                                    @class("position-absolute top-50 end-10 translate-middle-y ms-3 fa fa-search")
                                )
                            )
                        )
                    )
                ),
                main
                (
                    new[]
                    {
                        @class("pt-5"),
                        attribute("style", "padding-top: 5rem")
                    },
                    section
                    (
                        @class("ps-lg-4 pe-lg-3 pt-4"),
                        div
                        (
                            @class("row g-0 mx-n2"),
                            currentViewModel.Products.Select
                            (
                                product =>
                                {
                                    return
                                    div
                                    (
                                        @class("col-xl-2 col-lg-2 col-md-4 col-sm-6 col-xs-6 px-2 mb-3"),
                                        div
                                        (
                                            new[]
                                            {
                                                @class("card"),
                                                attribute("style", "border:0; transition: all .15s ease-in-out")
                                            },
                                            img
                                            (
                                                new[]
                                                {
                                                    @class("card-img-top d-block overflow-hidden"),
                                                    src(product.ImageSource)
                                                }
                                                    
                                            ),
                                            div
                                            (
                                                @class("card-body py-2"),
                                                h6
                                                (
                                                    @class("text-truncate"),
                                                    small
                                                    (
                                                        @class("text-muted"),
                                                        text
                                                        (
                                                            product.Title + $" ({product.UnitSize} {product.UnitOfMeasure})"
                                                        )
                                                    )
                                                ),
                                                div
                                                (
                                                    span
                                                    (
                                                        @class("text-accent"),
                                                        text
                                                        (
                                                            product.PriceUnits.ToString() + "."
                                                        ),
                                                        sup
                                                        (
                                                            text
                                                            (
                                                                product.PriceFraction.ToString() + " "
                                                            )
                                                        )
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
            );;

            //    form
            //    (
            //        attribute("onsubmit", "return false"),
            //        label
            //        (
            //            new[]
            //            {
            //                @class("mdc-text-field mdc-textfield--fullwidth mdc-text-field--with-leading-icon"),
            //                                    attribute("style", "width:100%"),
            //            },
            //            span
            //            (
            //                @class("mdc-text-field__ripple")
            //            ),
            //            i
            //            (
            //                new[]
            //                {
            //                    @class("material-icons mdc-text-field__icon mdc-text-field__icon--leading")
            //                },
            //                text("search")
            //            ),
            //            _searchInput,
            //            span
            //            (
            //                new[]
            //                {
            //                    @class("mdc-floating-label"),
            //                    id("search-label")
            //                },
            //                text
            //                (
            //                    "What are you looking for?"
            //                )
            //            ),

            //            span
            //            (
            //                @class("mdc-line-ripple")
            //            )
            //        ),
            //        script(text("new mdc.textField.MDCTextField(document.querySelector('.mdc-text-field'));")),
            //        div
            //        (
            //            @class("mdc-layout-grid"),
            //            div
            //            (
            //                @class("mdc-layout-grid__inner"),
            //                currentViewModel.Products.Select(product =>
            //                    div
            //                    (
            //                        @class("mdc-layout-grid__cell"),
            //                        div
            //                        (
            //                            new[]
            //                            {
            //                                @class("mdc-card"),
            //                                //attribute("style", "width: 350px; margin: 48px 0"),
            //                            },

            //                            div
            //                            (
            //                                @class("mdc-card__primary-action"),
            //                                div
            //                                (
            //                                    new[]
            //                                    {
            //                                        @class("mdc-card__media mdc-card__media--square"),
            //                                        attribute("style", $"background-size: auto; background-image: url({product.ImageSource})")

            //                                    }
            //                                    //,
            //                                    //svg
            //                                    //(
            //                                    //    attribute("xmlns", "http://www.w3.org/2000/svg"),
            //                                    //    attribute("xmlnsXlink", 

            //                                    //)
            //                                ),
            //                                div
            //                                (
            //                                    attribute("style", "padding: 1rem"),
            //                                    h2
            //                                    (
            //                                        @class("mdc-typography--title"),
            //                                        text
            //                                        (
            //                                            product.Title
            //                                        )
            //                                    ),
            //                                    h3
            //                                    (
            //                                        @class("mdc-typography--subtitle2"),
            //                                        text
            //                                        (
            //                                            product.PriceUnits.ToString() + "."
            //                                        ),
            //                                        sup
            //                                        (
            //                                            text
            //                                            (
            //                                                product.PriceFraction.ToString() + " "
            //                                            )
            //                                        ),
            //                                        text
            //                                        (
            //                                            $"{product.UnitSize} {product.UnitOfMeasure}"
            //                                        )
            //                                    )
            //                                )

            //                            )

            //                        )
            //                    )
            //            ).ToArray()
            //        )

            //    )
            ////div
            ////(
            ////    @class("row row-cols-auto g-1 "),
            ////    currentViewModel.Products.Select(product =>
            ////        div
            ////        (
            ////            @class("col-md-3"),
            ////            div
            ////            (
            ////                new[]
            ////                {
            ////                    @class("card p-3")
            ////                },
            ////                div
            ////                (
            ////                    @class("text-center"),
            ////                    img
            ////                    (
            ////                        new[]
            ////                        {
            ////                            @class("card-img-top"),
            ////                            src(product.ImageSource),
            ////                            width("200")
            ////                        }
            ////                    )
            ////                ),
            ////                div
            ////                (
            ////                    @class("card-body"),
            ////                    h6
            ////                    (
            ////                        @class("card-title"),
            ////                        text
            ////                        (
            ////                            product.Title
            ////                        )
            ////                    ),
            ////                    p
            ////                    (
            ////                        @class("card-text"),
            ////                        span
            ////                        (
            ////                            @class("font-weight-bold d-block"),
            ////                            text
            ////                            (
            ////                                product.PriceUnits.ToString()
            ////                            ),
            ////                            sup
            ////                            (
            ////                                text
            ////                                (
            ////                                    product.PriceFraction.ToString() + " "
            ////                                )
            ////                            )
            ////                        ),
            ////                        text
            ////                        (
            ////                            product.UnitOfMeasure
            ////                        )

            ////                    )
            ////                    //p
            ////                    //(
            ////                    //    @class("card-text"),
            ////                    //    small
            ////                    //    (
            ////                    //        @class("text-muted"),
            ////                    //        text
            ////                    //        (
            ////                    //            product.Price.Units
            ////                    //        ),
            ////                    //        sup
            ////                    //        (
            ////                    //            text
            ////                    //            (
            ////                    //                $",{product.Price.Fraction}"
            ////                    //            )
            ////                    //        ),
            ////                    //        text
            ////                    //        (
            ////                    //            product.UnitOfMeasure
            ////                    //        )
            ////                    //    )
            ////                    //),

            ////                )
            ////            )
            ////        )
            ////    ).ToArray()

            //)
            //    ); ;
        }
    }
}

﻿using Radix.Components;
using Radix.Components.Html;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;

namespace Radix.Shop.Catalog.Components
{
    public class Search : Component<SearchViewModel>
    {
        protected override Node View(SearchViewModel currentViewModel) =>
            concat
            (
                div
                (
                    @class("input-group"),
                    input
                    (
                        @class("form-control"),
                        placeholder("What are you looking for?"),
                        bind.input(currentViewModel.SearchTerm, searchTerm => currentViewModel.SearchTerm = searchTerm)
                    ),
                    button
                    (
                        new[]
                        {
                            @class("btn btn-outline-primary"),
                            type("button"),
                            on.click(async _ =>
                            {
                                currentViewModel.Products = new List<Product>();
                                await foreach(var product in currentViewModel.Search((SearchTerm)currentViewModel.SearchTerm))
                                {
                                    currentViewModel.Products.Add(product);
                                    StateHasChanged();
                                }   
                            })
                        },
                        text
                        (
                            "Search"
                        )
                    )
                ),
                div
                (
                    @class("row"),
                    currentViewModel.Products.Select(product =>
                        div
                        (
                            @class("col"),
                            div
                            (
                                new[]
                                {
                                    @class("card"),
                                    attribute("style", "width: 18rem;"),
                                },
                                img
                                (
                                    new[]
                                    {
                                        @class("card-img-top"),
                                        src(product.ImageSource)
                                    }
                                ),
                                div
                                (
                                    @class("card-body"),
                                    h5
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
                                        text
                                        (
                                            product.Price.Units.ToString()
                                        ),
                                        sup
                                        (
                                            text
                                            (
                                                $",{product.Price.Fraction}"
                                            )
                                        )

                                    ),
                                    button
                                    (
                                        new[]
                                        {
                                            @class("btn btn-primary"),
                                            on.click(_ => throw new NotImplementedException())
                                        },
                                        text
                                        (
                                            "Add to basket"
                                        )
                                    )
                                )
                            )
                        )
                    ).ToArray()
                )
            );
    }
}

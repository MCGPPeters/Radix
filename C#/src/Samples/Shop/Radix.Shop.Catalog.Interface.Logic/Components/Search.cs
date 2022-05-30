using Microsoft.AspNetCore.Components;
using Radix.Interaction;
using Radix.Interaction.Components;
using Radix.Interaction.Data;
using Radix.Shop.Catalog.Domain;
using System.Globalization;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

[Route("/catalog/search")]
public class Search : Component<SearchModel, SearchCommand>
{
    private Element? _searchInput;

    protected override Interaction.Update<SearchModel, SearchCommand> Update =>
        async (model, command) =>
        {
            model.Products = new List<ProductModel>();
            SearchTerm searchTerm = (SearchTerm)model.SearchTerm;
            await model.CrawlingMessageChannel.Writer.WriteAsync(searchTerm);
            await foreach (var product in model.Search(searchTerm))
            {
                model.Products.Add(product);
            }
            return model;
        };

    protected override View<SearchModel, SearchCommand> View =>
        async (model, dispatch) =>
        {
            _searchInput =
                input
                (
                    (NodeId)1,
                    @class((AttributeId)1, "form-control"),
                    placeholder((AttributeId)2, "Search for products"),
                    autocomplete((AttributeId)3, "off"),
                    autocapitalize((AttributeId)4, "off"),
                    attribute((AttributeId)5, "aria-labelledby", "search-label"),
                    type((AttributeId)6, "text"),
                    spellcheck((AttributeId)7, "false"),
                    attribute((AttributeId)8, "role", "textbox"),
                    bind.input((AttributeId)9, model.SearchTerm, searchTerm => model.SearchTerm = searchTerm),
                    on.keydown((AttributeId)10, async args =>
                    {
                        if (args.Key == "Enter")
                        {
                            dispatch(new SearchCommand(model.SearchTerm));
                        }
                    })
                );


            return concat
            (
                (NodeId)100,
                header
                (
                    (NodeId)2,
                    @class((AttributeId)11, "bg-light shadow-sm fixed-top data-fixed-element"),
                    div
                    (
                        (NodeId)3,
                        @class((AttributeId)12, "navbar navbar-expand-lg navbar-light shadow"),
                        div
                        (
                            (NodeId)4,
                            @class((AttributeId)12, "container-fluid"),
                            a
                            (
                                (NodeId)5,
                                new[]
                                {
                                @class((AttributeId) 13, "navbar-brand"),
                                href((AttributeId)13, "/")
                                },
                                img
                                (
                                    (NodeId)6,
                                    src((AttributeId)14, "https://via.placeholder.com/142x34/")
                                )
                            ),

                            div
                            (
                                (NodeId)7,
                                @class((AttributeId)15, "input-group"),
                                _searchInput,
                                button
                                (
                                    (NodeId)8,
                                    new[]
                                    {
                                        @class((AttributeId)16, "btn btn-large btn-primary"),
                                        type((AttributeId)17, "button"),
                                        on.click((AttributeId)18, async _ =>
                                        {
                                            dispatch(new SearchCommand(model.SearchTerm));
                                        })
                                    },
                                    i
                                    (
                                        (NodeId)9,
                                        @class((AttributeId)18, "bi", "bi-search")
                                    )
                                )
                            )
                        )
                    )
                ),
                main
                (
                    (NodeId)10,
                    new[]
                    {
                        @class((AttributeId) 19, "pt-5")
                    },
                    component<List>
                    (
                        (NodeId)11,
                        Enumerable.Empty<Interaction.Data.Attribute>()
                    ),
                    section
                    (
                        (NodeId)12,
                        @class((AttributeId)20, "ps-lg-4 pe-lg-3 pt-5"),
                        div
                        (
                            (NodeId)13,
                            @class
                            (
                                (AttributeId)21,
                                "row", "row-cols-2", "row-cols-md-4", "row-cols-lg-6", "g-0", "mx-n2"
                            ),
                            model.Products.Select
                            (
                                product =>
                                {
                                    return
                                    div
                                    (
                                        (NodeId)14,
                                        @class((AttributeId)22, "col g-4"),
                                        div
                                        (
                                            (NodeId)17,
                                            new[]
                                            {
                                                @class((AttributeId) 23, "card shadow-sm"),
                                                id((AttributeId) 24, "product-card"),
                                                attribute((AttributeId) 25, "style", "border:0; transition: all .15s ease-in-out"),

                                            },
                                            style
                                            (
                                                (NodeId)18,
                                                text
                                                (
                                                     (NodeId)19,
                                                    "#product-card:hover #product-title { text-overflow: initial; overflow: initial; white-space: initial;}" +
                                                    "#product-card:hover { transform: scale(1.05); box-shadow: 0 .5rem 1rem rgba(0,0,0,.15)!important;"
                                                )
                                            ),
                                            img
                                            (
                                                (NodeId)19,
                                                new[]
                                                {
                                                    @class((AttributeId) 26, "card-img-top d-block overflow-hidden p-3"),
                                                    src((AttributeId) 27, product.ImageSource)
                                                }

                                            ),
                                            div
                                            (
                                                 (NodeId)20,
                                                @class((AttributeId)28, "card-body", "py-2"),
                                                div
                                                (
                                                    (NodeId)21,
                                                    @class((AttributeId)29, "d-flex", "align-items-center", "justify-content-between"),
                                                    div
                                                    (
                                                        (NodeId)22,
                                                        p
                                                        (
                                                            (NodeId)23,
                                                            @class((AttributeId)30, "text-accent", "mb-0"),
                                                            text
                                                            (
                                                                (NodeId)24,
                                                                product.PriceUnits + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator
                                                            ),
                                                            sup
                                                            (
                                                                 (NodeId)25,
                                                                text
                                                                (
                                                                     (NodeId)26,
                                                                    product.PriceFraction
                                                                )
                                                            )
                                                        ),
                                                        p
                                                        (
                                                             (NodeId)27,
                                                            attribute((AttributeId)31, "style", "font-size: .6em"),
                                                            text
                                                            (
                                                                (NodeId)28,
                                                                $"{product.UnitSize} {product.UnitOfMeasure}"
                                                            )
                                                        )
                                                    ),

                                                    product.MerchantName switch
                                                    {
                                                        "Albert Heijn" =>
                                                            component<AH.LogoReference>((NodeId)101, Enumerable.Empty<Interaction.Data.Attribute>()),
                                                        "Jumbo" =>
                                                            component<Jumbo.LogoReference>((NodeId)102, Enumerable.Empty<Interaction.Data.Attribute>()),
                                                        _ => throw new NotImplementedException()

                                                    }
                                                    ,
                                                    div
                                                    (
                                                        (NodeId)29,
                                                        p
                                                        (
                                                            (NodeId)30,
                                                            @class((AttributeId)32, "text-accent mb-0"),
                                                            text
                                                            (
                                                                (NodeId)31,
                                                                product.PricePerUnitPriceUnits + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator
                                                            ),
                                                            sup
                                                            (
                                                                (NodeId)32,
                                                                text
                                                                (
                                                                    (NodeId)32,
                                                                    product.PricePerUnitPriceFraction
                                                                )
                                                            )
                                                        ),
                                                        p
                                                        (
                                                            (NodeId)33,
                                                            attribute((AttributeId)3, "style", "font-size: .6em"),
                                                            text
                                                            (
                                                                (NodeId)34,
                                                                $"/ {product.UnitOfMeasure}"
                                                            )
                                                        )
                                                    )
                                                ),
                                                h6
                                                (
                                                    (NodeId)34,
                                                    new[]
                                                    {
                                                        @class((AttributeId) 34, "text-truncate", "card-title"),
                                                        id((AttributeId) 35, "product-title")
                                                    },
                                                    text
                                                    (
                                                        (NodeId)42,
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
        };


    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (_searchInput is not null && firstRender)
            await _searchInput.ElementReference.FocusAsync();
    }

}

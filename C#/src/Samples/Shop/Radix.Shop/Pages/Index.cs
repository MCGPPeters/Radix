using Microsoft.AspNetCore.Components;
using Radix.Components;
using Radix.Components.Html;
using Radix.Shop.Sales;

namespace Radix.Shop.Pages
{
    [Route("/")]
    public class Index : Component<IndexViewModel>
    {
        protected override Node View(IndexViewModel currentViewModel) =>
            concat
            (
                h1
                (
                    text
                    (
                        "Products"
                    )
                ),
                div
                (
                    @class("container-fluid"),
                    div
                    (
                        @class("row"),
                        div
                        (
                            @class("col"),
                            div
                            (
                                @class("form-group"),
                                label
                                (
                                    @for("brandFilter"),
                                    text
                                    (
                                        "Brand"
                                    )
                                ),
                                select
                                (
                                    new[]
                                    {
                                        @class("form-control"),
                                        id("brandFilter"),
                                        bind.change(currentViewModel.BrandFilter, brand => currentViewModel.TypeFilter = brand)
                                    },
                                    RenderBrandOptions(currentViewModel.Brands)
                                ),
                                label
                                (
                                    @for("typeFilter"),
                                    text
                                    (
                                        "Type"
                                    )
                                ),
                                select
                                (
                                    new[]
                                    {
                                        @class("form-control"),
                                        id("typeFilter"),
                                        bind.change(currentViewModel.TypeFilter, type => currentViewModel.TypeFilter = type)
                                    },
                                    RenderTypeOptions(currentViewModel.Types)
                                ),
                                button
                                (
                                    new[]
                                    {
                                        @class("btn btn-primary"),
                                        on.click
                                        (
                                            async _ =>
                                            {
                                                currentViewModel.FilteredProducts = new List<Product>();
                                                await foreach (Product product in currentViewModel.GetFilteredProducts(currentViewModel.Brands, currentViewModel.Types))
                                               currentViewModel.FilteredProducts.Add(product);
                                            }
                                        )
                                    },
                                    text
                                    (
                                        "Search"
                                    )
                                )
                            )
                        )
                    ),
                    div
                    (
                        @class("row"),
                        RendedFilteredProducts(currentViewModel)
                    )      
                )
            );

        internal static Node[] RenderBrandOptions(IEnumerable<Brand> brands) =>
            brands.Select(brand =>
                option
                (
                    value(brand),
                    text
                    (
                        brand
                    )   
                )
            ).ToArray();

        internal static Node[] RenderTypeOptions(IEnumerable<ProductType> types) =>
            types.Select(type =>
                option
                (
                    value(type),
                    text
                    (
                        type
                    )
                )
            ).ToArray();

        private static Node[] RendedFilteredProducts(IndexViewModel viewModel) =>
            viewModel.FilteredProducts.Select(product =>
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
                                src("/images/products/1.png")
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
                                    product.ProductName
                                )
                            ),
                            p
                            (
                                @class("card-text"),
                                text
                                (
                                    product.Price.ToString("N2")
                                )
                            ),
                            button
                            (
                                new[]
                                {
                                    @class("btn btn-primary"),     
                                    on.click(_ => viewModel.AddToBasket(product))
                                },                               
                                text
                                (
                                    "Add to basket"
                                )
                            )
                         )
                    )
                )
            ).ToArray();
            
    }
}

using Microsoft.AspNetCore.Components;
using Radix.Components;
using Radix.Components.Html;

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
                        "Product"
                    )
                ),
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
                                currentViewModel.ApplyFilter()
                            )
                        },
                        text
                        (
                            "Search"
                        )
                    )
                ),
                div
                (
                    @class(""),
                    RendedFilteredProducts(currentViewModel.FilteredProducts)
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
        internal static Node[] GetFilteredProducts() => Array.Empty<Node>();
        internal static Node[] RenderTypeOptions(IEnumerable<Type> types) =>
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

        private static Node[] RendedFilteredProducts(IEnumerable<Product> filteredProducts) => Array.Empty<Node>();
    }
}

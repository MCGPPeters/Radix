using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Components;
using Radix.Components.Html;
using Radix.Shop.Catalog.Interface.Logic.Components;
using Radix.Shop.Wasm.Pages;

namespace Radix.Shop.Pages
{
    [Route("/")]
    public class Index : Component<IndexViewModel>
    {
        protected override Node View(IndexViewModel currentViewModel) =>
            concat
            (
                component<Carousel>
                (
                    Array.Empty<IAttribute>(),
                    Array.Empty<Node>()
                )
            );
    }
    
}

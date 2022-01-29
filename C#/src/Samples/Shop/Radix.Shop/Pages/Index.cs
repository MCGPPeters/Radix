using Microsoft.AspNetCore.Components;
using Radix.Components;
using Radix.Components.Html;
using Radix.Shop.Catalog.Interface.Logic.Components;

namespace Radix.Shop.Pages
{
    [Route("/")]
    public class Index : Component<IndexViewModel>
    {
        protected override Node View(IndexViewModel currentViewModel) =>
            concat
            (
                component<Search>
                (
                    Array.Empty<ComponentAttribute>(),
                    Array.Empty<Node>()
                )
            );          
    }
}

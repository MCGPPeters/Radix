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
                component<Catalog.Components.Search>
                (
                    new IAttribute[] { },
                    new Node[] { }
                )
            );


        
            
    }
}

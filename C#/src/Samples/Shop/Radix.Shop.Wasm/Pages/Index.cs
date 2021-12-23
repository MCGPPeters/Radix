using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Radix.Components;
using Radix.Components.Html;
using Radix.Shop.Catalog.Interface.Logic.Components;
using Radix.Shop.Wasm.Pages;

namespace Radix.Shop.Pages
{
    [Route("/")]
    public class Index : Radix.Components.Component
    {
        protected override Node View() =>
            text("Hello!");
    }
    
}

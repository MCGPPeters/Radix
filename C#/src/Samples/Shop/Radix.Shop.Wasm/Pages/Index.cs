using Microsoft.AspNetCore.Components;
using Radix.Components.Html;

namespace Radix.Shop.Wasm.Pages
{
    [Route("/")]
    public class Index : Radix.Components.Component
    {
        protected override Node View() =>
            text("Hello!");
    }
    
}

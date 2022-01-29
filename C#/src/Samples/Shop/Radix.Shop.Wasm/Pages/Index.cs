using Microsoft.AspNetCore.Components;
using Radix.Components;
using static Radix.Components.Prelude;

namespace Radix.Shop.Wasm.Pages
{
    [Route("/")]
    public class Index : Component
    {
        protected override Node View() =>
            text("Hello!");
    }
    
}

using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Elements;

namespace Radix.Blazor.Inventory.Pages
{
    [Route("/counter")]
    public class CounterComponent : Component<AddInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        private int currentCount;

        private void IncrementCount()
        {
            currentCount++;
        }

        public override Node Render(AddInventoryItemViewModel currentViewModel)
        {
            return concat(
                h1(Enumerable.Empty<IAttribute>(), text("Counter")),
                p(Enumerable.Empty<IAttribute>(), text(currentCount.ToString())),
                button(
                    new[]
                    {
                        @class("btn", "btn-primary"), on.click(
                            args => { IncrementCount(); })
                    },
                    text("Click me")));
        }
    }
}

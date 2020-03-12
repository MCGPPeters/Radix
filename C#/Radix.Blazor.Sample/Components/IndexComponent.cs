using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Elements;

namespace Radix.Blazor.Sample.Components
{
    [Route("/")]
    // ReSharper disable once ClassNeverInstantiated.Global : Instantiated via DI
    public class IndexComponent : BoundedContextComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext, IndexViewModel currentViewModel)
        {
            var InventoryItemNodes = GetInventoryItemNodes(currentViewModel.InventoryItems);

            return concat(
                h1(Enumerable.Empty<IAttribute>(), text("All items")),
                    ul(Enumerable.Empty<IAttribute>(), InventoryItemNodes)
            );
        }

        private static Node[] GetInventoryItemNodes(IEnumerable<(Address, string name)> inventoryItems)
        {
            return inventoryItems.Select(
                inventoryItem =>
                    li(Enumerable.Empty<IAttribute>(), text(inventoryItem.name))).ToArray();
        }
    }
}

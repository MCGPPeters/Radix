using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Components;
using static Radix.Blazor.Html.Elements;

namespace Radix.Blazor.Sample.Components
{
    [Route("/")]
    // ReSharper disable once ClassNeverInstantiated.Global : Instantiated via DI
    public class IndexComponent : BoundedContextComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext, IndexViewModel currentViewModel)
        {
            Node[] InventoryItemNodes = GetInventoryItemNodes(currentViewModel.InventoryItems);

            return concat(
                navLinkMatchAll(new[] { @class("btn btn-primary"), href("Add") }, text("Add")),
                h1(Enumerable.Empty<IAttribute>(), text("All items")),
                ul(Enumerable.Empty<IAttribute>(), InventoryItemNodes)
            );
        }

        private static Node[] GetInventoryItemNodes(IEnumerable<(Address address, string name)> inventoryItems)
        {
            return inventoryItems.Select(
                inventoryItem =>
                    li(Enumerable.Empty<IAttribute>(), 
                        navLinkMatchAll(new[] { href($"/Details/{inventoryItem.address}") }, 
                            text(inventoryItem.name)))).ToArray();
        }
    }
}

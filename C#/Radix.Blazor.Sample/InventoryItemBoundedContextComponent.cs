using System;
using System.Collections.Generic;
using System.Linq;
using Radix.Blazor.Html;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Attributes;

namespace Radix.Blazor.Sample
{
    // ReSharper disable once ClassNeverInstantiated.Global : Instantiated via DI
    public class InventoryItemBoundedContextComponent : BoundedContextComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext, IndexViewModel currentViewModel)
        {
            var InventoryItemNodes = GetInventoryItemNodes(currentViewModel.InventoryItems);

            return concat(
                div(
                    new[] { @class("container") },
                        nav(new[] { @class("navbar navbar-expand-lg navbar-light bg-light") },
                            a(new[] { @class("navbar-brand"), href("#") },
                                text("Inventory"))),
                    div(new[] { @class("row") },
                        ul(Enumerable.Empty<IAttribute>(), InventoryItemNodes))
            ));
        }

        private static Node[] GetInventoryItemNodes(IEnumerable<(Address, string name)> inventoryItems)
        {
            return inventoryItems.Select(inventoryItem => li(Enumerable.Empty<IAttribute>(), text(inventoryItem.name))).ToArray();
        }
    }
}

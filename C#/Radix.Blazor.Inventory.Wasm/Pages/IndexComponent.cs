using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Components;

namespace Radix.Blazor.Inventory.Wasm.Pages
{
    [Route("/")]
    public class IndexComponent : Component<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        public override Node View(IndexViewModel currentViewModel)
        {
            Node[] inventoryItemNodes = GetInventoryItemNodes(currentViewModel.InventoryItems);

            return concat(
                navLinkMatchAll(new[] {@class("btn btn-primary"), href("Add")}, text("Add")),
                h1(Enumerable.Empty<IAttribute>(), text("All items")),
                ul(Enumerable.Empty<IAttribute>(), inventoryItemNodes)
            );
        }

        private static Node[] GetInventoryItemNodes(IEnumerable<(Address address, string name)> inventoryItems) => inventoryItems.Select(
            inventoryItem =>
                li(
                    Enumerable.Empty<IAttribute>(),
                    navLinkMatchAll(
                        new[] {href($"/Details/{inventoryItem.address}")},
                        text(inventoryItem.name)))).ToArray();
    }
}

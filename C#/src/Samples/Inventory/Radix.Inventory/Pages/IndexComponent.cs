using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Components;
using Radix.Components.Html;
using Radix.Inventory.Domain;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;
using static Radix.Components.Html.Components;
using Radix.Option;

namespace Radix.Blazor.Inventory.Server.Pages
{
    [Route("/")]
    public class IndexComponent : TaskBasedComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent, Json>
    {

        protected override Update<IndexViewModel, InventoryItemEvent> Update { get; } =
            (state, events) => state;


        protected override Node View(IndexViewModel currentViewModel)
        {
            Node[] inventoryItemNodes = GetInventoryItemNodes(currentViewModel.InventoryItems);

            return concat(
                navLinkMatchAll(new[] { @class("btn btn-primary"), href("Add") }, text("Add")),
                h1(None, text("All items")),
                table(None, inventoryItemNodes)
            );
        }


        private static Node[] GetInventoryItemNodes(IEnumerable<InventoryItemModel> inventoryItems) =>
            inventoryItems.Select(
                inventoryItem =>
                    tr(
                        None,
                        td(
                            None,
                            navLinkMatchAll(
                                new[] { href($"/Details/{inventoryItem.id}") },
                                text(inventoryItem.name))),
                        // conditional formating
                        inventoryItem.activated
                        ? td(
                            None,
                            navLinkMatchAll(
                                new[] { href($"/Deactivate/{inventoryItem.id}") },
                                text("Deactivate")))
                        : td(
                            None,
                            navLinkMatchAll(
                                new[] { href($"/Activate/{inventoryItem.id}") },
                                text("Activate"))))).ToArray();
    }
}

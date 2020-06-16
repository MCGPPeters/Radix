using System;
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

namespace Radix.Blazor.Inventory.Server.Pages
{
    [Route("/")]
    public class IndexComponent : Component<IndexViewModel, InventoryItemCommand, InventoryItemEvent, Json>
    {

        protected override Update<IndexViewModel, InventoryItemEvent> Update { get; } =

            (state, events) =>
            {
                return events.Aggregate(
                    state,
                    (model, @event) =>
                    {
                        switch (@event)
                        {
                            case InventoryItemCreated inventoryItemCreated:
                                state.InventoryItems.Add((inventoryItemCreated.Address, inventoryItemCreated.Name));
                                break;
                            case InventoryItemDeactivated _:
                                break;
                            case InventoryItemRenamed inventoryItemRenamed:
                                state.InventoryItems = state.InventoryItems
                                    .Select(_ => (@event.Address, inventoryItemRenamed.Name))
                                    .Where(tuple => tuple.Address.Equals(@event.Address)).ToList();
                                break;
                            case ItemsCheckedInToInventory _:
                                break;
                            case ItemsRemovedFromInventory _:
                                break;
                            default:
                                throw new NotSupportedException("Unknown event");
                        }

                        return state;
                    });
            };

        protected override Node View(IndexViewModel currentViewModel)
        {
            Node[] inventoryItemNodes = GetInventoryItemNodes(currentViewModel.InventoryItems);

            return concat(
                navLinkMatchAll(new[] {@class("btn btn-primary"), href("Add")}, text("Add")),
                h1(NoAttributes(), text("All items")),
                table(Enumerable.Empty<IAttribute>(), inventoryItemNodes)
            );
        }

        private static IEnumerable<IAttribute> NoAttributes() => Enumerable.Empty<IAttribute>();

        private static Node[] GetInventoryItemNodes(IEnumerable<(Address address, string name)> inventoryItems) => inventoryItems.Select(
            inventoryItem =>
                tr(
                    Enumerable.Empty<IAttribute>(),
                    td(NoAttributes(), 
                    navLinkMatchAll(
                        new[] {href($"/Details/{inventoryItem.address}")},
                        text(inventoryItem.name))),
                    td(NoAttributes(), navLinkMatchAll(
                        new[] { href($"/Deactivate/{inventoryItem.address}") },
                        text("Deactivate"))))).ToArray();
    }
}

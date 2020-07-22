using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Radix.Result.Extensions;

namespace Radix.Inventory.Domain
{
    public record InventoryItem
    {
        public static Update<InventoryItem, InventoryItemEvent> Update = (state, events) =>
        {
            return events.Aggregate(
                state,
                (item, @event) =>
                {
                    return @event switch
                    {
                        InventoryItemCreated inventoryItemCreated => state with { Name = inventoryItemCreated.Name },
                        InventoryItemDeactivated inventoryItemDeactivated => state with { Activated = true,   ReasonForDeactivation = inventoryItemDeactivated.Reason},
                        ItemsCheckedInToInventory itemsCheckedInToInventory => state with { Count = state.Count + itemsCheckedInToInventory.Amount },
                        ItemsRemovedFromInventory itemsRemovedFromInventory => state with { Count = state.Count - itemsRemovedFromInventory.Amount },
                        InventoryItemRenamed inventoryItemRenamed => state with { Name = inventoryItemRenamed.Name },
                        _ => throw new NotSupportedException("Unknown event")
                    };
                });
        };

        public string ReasonForDeactivation { get; init; }

        public static Decide<InventoryItem, InventoryItemCommand, InventoryItemEvent> Decide = (state, command) =>
        {
            return command switch
            {
                DeactivateInventoryItem deactivateInventoryItem => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(new InventoryItemEvent[] {new InventoryItemDeactivated(deactivateInventoryItem.Reason)})),
                CreateInventoryItem createInventoryItem => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(
                        new InventoryItemEvent[]
                        {
                            new InventoryItemCreated(createInventoryItem.Id, createInventoryItem.Name, createInventoryItem.Activated, createInventoryItem.Count)
                        })),
                RenameInventoryItem renameInventoryItem => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(new InventoryItemEvent[] { new InventoryItemRenamed { Id = renameInventoryItem.Id, Name = renameInventoryItem.Name } })),
                CheckInItemsToInventory checkInItemsToInventory => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(new InventoryItemEvent[] { new ItemsCheckedInToInventory { Amount = checkInItemsToInventory.Amount, Id = checkInItemsToInventory.Id } })),
                RemoveItemsFromInventory removeItemsFromInventory => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(
                        new InventoryItemEvent[] {new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, removeItemsFromInventory.Id)})),
                _ => throw new NotSupportedException("Unknown transientCommand")
            };
        };

        public InventoryItem()
        {
            Name = "";
            Activated = true;
            Count = 0;
            ReasonForDeactivation = "";
        }

        public InventoryItem(string name, bool activated, int count)
        {
            Name = name;
            Activated = activated;
            Count = count;
            ReasonForDeactivation = "";
        }

        public string Name { get; init; }
        public bool Activated { get; init; }
        public int Count { get; init; }
    }
}

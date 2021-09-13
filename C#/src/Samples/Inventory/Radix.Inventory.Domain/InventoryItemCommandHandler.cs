using static Radix.Result.Extensions;

namespace Radix.Inventory.Domain;

public class InventoryItemCommandHandler : CommandHandler<InventoryItem, InventoryItemCommand, InventoryItemEvent, InventoryItemCommandHandler>
{
    public static Update<InventoryItem, InventoryItemEvent> Update
    {
        get => (state, events) =>
        {
            return events.Aggregate(
                state,
                (item, @event) =>
                {
                    return @event switch
                    {
                        InventoryItemCreated inventoryItemCreated => state with { Name = inventoryItemCreated.Name },
                        InventoryItemDeactivated inventoryItemDeactivated => state with { Activated = false, ReasonForDeactivation = inventoryItemDeactivated.Reason },
                        ItemsCheckedInToInventory itemsCheckedInToInventory => state with { Count = state.Count + itemsCheckedInToInventory.Amount },
                        ItemsRemovedFromInventory itemsRemovedFromInventory => state with { Count = state.Count - itemsRemovedFromInventory.Amount },
                        InventoryItemRenamed inventoryItemRenamed => state with { Name = inventoryItemRenamed.Name },
                        _ => throw new NotSupportedException("Unknown event")
                    };
                });
        };
    }

    public static Decide<InventoryItem, InventoryItemCommand, InventoryItemEvent> Decide
    {
        get => (_, command) =>
        {
            return command switch
            {
                DeactivateInventoryItem deactivateInventoryItem => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(new InventoryItemEvent[] { new InventoryItemDeactivated(deactivateInventoryItem.Reason) })),
                CreateInventoryItem createInventoryItem => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(
                        new InventoryItemEvent[]
                        {
                                new InventoryItemCreated(createInventoryItem.Id, createInventoryItem.Name, createInventoryItem.Activated, createInventoryItem.Count)
                        })),
                RenameInventoryItem renameInventoryItem => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(
                        new InventoryItemEvent[] { new InventoryItemRenamed { Id = renameInventoryItem.Id, Name = renameInventoryItem.Name } })),
                CheckInItemsToInventory checkInItemsToInventory => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(
                        new InventoryItemEvent[] { new ItemsCheckedInToInventory { Amount = checkInItemsToInventory.Amount, Id = checkInItemsToInventory.Id } })),
                RemoveItemsFromInventory removeItemsFromInventory => Task.FromResult(
                    Ok<InventoryItemEvent[], CommandDecisionError>(
                        new InventoryItemEvent[] { new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, removeItemsFromInventory.Id) })),
                _ => throw new NotSupportedException("Unknown transientCommand")
            };
        };
    }
}

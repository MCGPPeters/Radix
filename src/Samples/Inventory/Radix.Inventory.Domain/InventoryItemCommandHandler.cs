using Radix.Domain.Control;
using Radix.Domain.Data;
using Radix.Inventory.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using static Radix.Control.Result.Extensions;

namespace Radix.Inventory.Domain;

public class ItemCommandHandler : CommandHandler<Item, ItemCommand, ItemEvent>
{
    public static Update<Item, ItemEvent> Update
    {
        get => (state, events) =>
        {
            return events.Aggregate(
                state,
                (item, @event) =>
                {
                    return @event switch
                    {
                        ItemCreated inventoryItemCreated => state with { Name = inventoryItemCreated.Name },
                        ItemDeactivated inventoryItemDeactivated => state with { Activated = false, ReasonForDeactivation = inventoryItemDeactivated.Reason },
                        ItemsCheckedInToInventory itemsCheckedInToInventory => state with { Count = state.Count + itemsCheckedInToInventory.Amount },
                        ItemsRemovedFromInventory itemsRemovedFromInventory => state with { Count = state.Count - itemsRemovedFromInventory.Amount },
                        ItemRenamed inventoryItemRenamed => state with { Name = inventoryItemRenamed.Name },
                        _ => throw new NotSupportedException("Unknown event")
                    };
                });
        };
    }

    public static Decide<Item, ItemCommand, ItemEvent> Decide
    {
        get => (_, command) =>
        {
            return command switch
            {
                DeactivateItem deactivateInventoryItem => Task.FromResult(
                    Ok<ItemEvent[], CommandDecisionError>(new ItemEvent[] { new ItemDeactivated(deactivateInventoryItem.Reason) })),
                CreateItem createInventoryItem => Task.FromResult(
                    Ok<ItemEvent[], CommandDecisionError>(
                        new ItemEvent[]
                        {
                                new ItemCreated(createInventoryItem.Id, createInventoryItem.Name, createInventoryItem.Activated, createInventoryItem.Count)
                        })),
                RenameItem renameInventoryItem => Task.FromResult(
                    Ok<ItemEvent[], CommandDecisionError>(
                        new ItemEvent[] { new ItemRenamed { Id = renameInventoryItem.Id, Name = renameInventoryItem.Name } })),
                CheckInItemsToInventory checkInItemsToInventory => Task.FromResult(
                    Ok<ItemEvent[], CommandDecisionError>(
                        new ItemEvent[] { new ItemsCheckedInToInventory { Amount = checkInItemsToInventory.Amount, Id = checkInItemsToInventory.Id } })),
                RemoveItemsFromInventory removeItemsFromInventory => Task.FromResult(
                    Ok<ItemEvent[], CommandDecisionError>(
                        new ItemEvent[] { new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, removeItemsFromInventory.Id) })),
                _ => throw new NotSupportedException("Unknown transientCommand")
            };
        };
    }
}

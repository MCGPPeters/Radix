using Radix.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Radix.Tests;

namespace Radix.Inventory.Domain.Data;

public record Item : Aggregate<Item, InventoryCommand, InventoryEvent>
{
    public static Item Create()
    {
        return new()
        {
            Name = "",
            Activated = true,
            Count = 0,
            ReasonForDeactivation = "",
        };

    }

    public string? ReasonForDeactivation { get; init; }
    public string? Name { get; init; }
    public bool Activated { get; init; }
    public required int Count { get; init; }
    public static string Id => "Item";

    public static Item Apply(Item state, InventoryEvent @event) =>
        @event switch
        {
            ItemCreated inventoryItemCreated => state with { Name = inventoryItemCreated.Name, Count = inventoryItemCreated.Count },
            ItemDeactivated inventoryItemDeactivated => state with { Activated = false, ReasonForDeactivation = inventoryItemDeactivated.Reason },
            ItemsCheckedInToInventory itemsCheckedInToInventory => state with { Count = state.Count + itemsCheckedInToInventory.Amount },
            ItemsRemovedFromInventory itemsRemovedFromInventory => state with { Count = state.Count - itemsRemovedFromInventory.Amount },
            ItemRenamed inventoryItemRenamed => state with { Name = inventoryItemRenamed.Name },
            _ => throw new NotSupportedException("Unknown event")
        };

    public static IEnumerable<InventoryEvent> Decide(Item state, InventoryCommand command) =>
        command switch
        {
            DeactivateItem deactivateInventoryItem => new InventoryEvent[] { new ItemDeactivated(deactivateInventoryItem.Reason) },
            CreateItem createInventoryItem =>
                new InventoryEvent[] { new ItemCreated(createInventoryItem.Id, createInventoryItem.Name, createInventoryItem.Activated, createInventoryItem.Count) },
            RenameItem renameInventoryItem =>
                new InventoryEvent[] { new ItemRenamed { Id = renameInventoryItem.Id, Name = renameInventoryItem.Name } },
            CheckInItemsToInventory checkInItemsToInventory =>
                new InventoryEvent[] { new ItemsCheckedInToInventory { Amount = checkInItemsToInventory.Amount, Id = checkInItemsToInventory.Id } },
            RemoveItemsFromInventory removeItemsFromInventory =>
                new InventoryEvent[] { new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, removeItemsFromInventory.Id) },
            _ => throw new NotSupportedException("Unknown transientCommand")
        };

    public static IAsyncEnumerable<InventoryEvent> ResolveConflicts(Item state, IEnumerable<InventoryEvent> ourEvents, IOrderedAsyncEnumerable<Event<InventoryEvent>> theirEvents) => ourEvents.ToAsyncEnumerable();
}

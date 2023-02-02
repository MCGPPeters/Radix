using Radix.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Radix.Tests;

namespace Radix.Inventory.Domain.Data;

public record Item : Aggregate<Item, ItemCommand, ItemEvent>
{
    public Item()
    {
        Name = "";
        Activated = true;
        Count = 0;
        ReasonForDeactivation = "";
    }

    public Item(string name, bool activated, int count)
    {
        Name = name;
        Activated = activated;
        Count = count;
        ReasonForDeactivation = "";
    }

    public string? ReasonForDeactivation { get; init; }
    public string? Name { get; init; }
    public bool Activated { get; init; }
    public int Count { get; init; }
    public static string Id => "Item";

    public static Item Apply(Item state, ItemEvent @event) =>
        @event switch
        {
            ItemCreated inventoryItemCreated => state with { Name = inventoryItemCreated.Name, Count = inventoryItemCreated.Count },
            ItemDeactivated inventoryItemDeactivated => state with { Activated = false, ReasonForDeactivation = inventoryItemDeactivated.Reason },
            ItemsCheckedInToInventory itemsCheckedInToInventory => state with { Count = state.Count + itemsCheckedInToInventory.Amount },
            ItemsRemovedFromInventory itemsRemovedFromInventory => state with { Count = state.Count - itemsRemovedFromInventory.Amount },
            ItemRenamed inventoryItemRenamed => state with { Name = inventoryItemRenamed.Name },
            _ => throw new NotSupportedException("Unknown event")
        };

    public static IEnumerable<ItemEvent> Decide(Item state, ItemCommand command) =>
        command switch
        {
            DeactivateItem deactivateInventoryItem => new ItemEvent[] { new ItemDeactivated(deactivateInventoryItem.Reason) },
            CreateItem createInventoryItem =>
                new ItemEvent[] { new ItemCreated(createInventoryItem.Id, createInventoryItem.Name, createInventoryItem.Activated, createInventoryItem.Count) },
            RenameItem renameInventoryItem =>
                new ItemEvent[] { new ItemRenamed { Id = renameInventoryItem.Id, Name = renameInventoryItem.Name } },
            CheckInItemsToInventory checkInItemsToInventory =>
                new ItemEvent[] { new ItemsCheckedInToInventory { Amount = checkInItemsToInventory.Amount, Id = checkInItemsToInventory.Id } },
            RemoveItemsFromInventory removeItemsFromInventory =>
                new ItemEvent[] { new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, removeItemsFromInventory.Id) },
            _ => throw new NotSupportedException("Unknown transientCommand")
        };

    public static IAsyncEnumerable<ItemEvent> ResolveConflicts(Item state, IEnumerable<ItemEvent> ourEvents, IOrderedAsyncEnumerable<Event<ItemEvent>> theirEvents) => ourEvents.ToAsyncEnumerable();
}

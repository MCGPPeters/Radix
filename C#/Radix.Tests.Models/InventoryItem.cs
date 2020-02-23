using System;
using System.Collections.Generic;

namespace Radix.Tests.Models
{
    public class InventoryItem : Aggregate<InventoryItem, InventoryItemEvent, InventoryItemCommand>
    {
        
        public InventoryItem()
        {
            Name = "";
            Activated = true;
            Count = 0;
        }

        public InventoryItem(string name, bool activated, int count)
        {
            Name = name;
            Activated = activated;
            Count = count;
        }

        private string Name { get; }
        private bool Activated { get; }
        private int Count { get; }

        public InventoryItemEvent[] Decide(CommandDescriptor<InventoryItemCommand> commandDescriptor) => commandDescriptor.Command switch
        {
            DeactivateInventoryItem _ => new [] {new InventoryItemDeactivated(commandDescriptor.Address)},
            CreateInventoryItem createInventoryItem => new[] { new InventoryItemCreated(createInventoryItem.Name, createInventoryItem.Activated, createInventoryItem.Count, commandDescriptor.Address) },
            RenameInventoryItem renameInventoryItem => new[] { new InventoryItemRenamed(renameInventoryItem.Name, commandDescriptor.Address) },
            CheckInItemsToInventory checkInItemsToInventory => new[] { new ItemsCheckedInToInventory(checkInItemsToInventory.Amount, commandDescriptor.Address) },
            RemoveItemsFromInventory removeItemsFromInventory => new[] { new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, commandDescriptor.Address) },
            _ => throw new NotSupportedException("Unknown commandDescriptor")
        };


        public InventoryItem Apply(InventoryItemEvent @event) => @event switch
        {
            InventoryItemCreated inventoryItemCreated => new InventoryItem(inventoryItemCreated.Name, Activated, Count),
            InventoryItemDeactivated _ => new InventoryItem(Name, false, Count),
            ItemsCheckedInToInventory itemsCheckedInToInventory => new InventoryItem(Name, Activated, Count + itemsCheckedInToInventory.Amount),
            ItemsRemovedFromInventory itemsRemovedFromInventory => new InventoryItem(Name, Activated, Count - itemsRemovedFromInventory.Amount),
            InventoryItemRenamed inventoryItemRenamed => new InventoryItem(inventoryItemRenamed.Name, Activated, Count),
            _ => throw new NotSupportedException("Unknown event")
        };
    }
}

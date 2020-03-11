using System;

namespace Radix.Tests.Models
{
    public class InventoryItem : Aggregate<InventoryItem, InventoryItemEvent, InventoryItemCommand>, IEquatable<InventoryItem>
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

        public InventoryItemEvent[] Decide(CommandDescriptor<InventoryItemCommand> commandDescriptor)
        {
            return commandDescriptor.Command switch
            {
                DeactivateInventoryItem _ => new InventoryItemEvent[] {new InventoryItemDeactivated(commandDescriptor.Address)},
                CreateInventoryItem createInventoryItem => new InventoryItemEvent[]
                    {new InventoryItemCreated(createInventoryItem.Name, createInventoryItem.Activated, createInventoryItem.Count, commandDescriptor.Address)},
                RenameInventoryItem renameInventoryItem => new InventoryItemEvent[] {new InventoryItemRenamed(renameInventoryItem.Name, commandDescriptor.Address)},
                CheckInItemsToInventory checkInItemsToInventory => new InventoryItemEvent[]
                    {new ItemsCheckedInToInventory(checkInItemsToInventory.Amount, commandDescriptor.Address)},
                RemoveItemsFromInventory removeItemsFromInventory => new InventoryItemEvent[]
                    {new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, commandDescriptor.Address)},
                _ => throw new NotSupportedException("Unknown command")
            };
        }


        public InventoryItem Apply(InventoryItemEvent @event)
        {
            return @event switch
            {
                InventoryItemCreated inventoryItemCreated => new InventoryItem(inventoryItemCreated.Name, Activated, Count),
                InventoryItemDeactivated _ => new InventoryItem(Name, false, Count),
                ItemsCheckedInToInventory itemsCheckedInToInventory => new InventoryItem(Name, Activated, Count + itemsCheckedInToInventory.Amount),
                ItemsRemovedFromInventory itemsRemovedFromInventory => new InventoryItem(Name, Activated, Count - itemsRemovedFromInventory.Amount),
                InventoryItemRenamed inventoryItemRenamed => new InventoryItem(inventoryItemRenamed.Name, Activated, Count),
                _ => throw new NotSupportedException("Unknown event")
            };
        }

        public bool Equals(InventoryItem? other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return Name == other.Name && Activated == other.Activated && Count == other.Count;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((InventoryItem) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Activated, Count);
        }

        public static bool operator ==(InventoryItem? left, InventoryItem? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InventoryItem? left, InventoryItem? right)
        {
            return !Equals(left, right);
        }
    }
}

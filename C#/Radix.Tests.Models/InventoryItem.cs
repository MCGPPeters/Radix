using System;
using System.Linq;
using System.Threading.Tasks;
using Radix.Validated;
using static Radix.Validated.Extensions;
using static Radix.Result.Extensions;

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

        public Task<Result<InventoryItemEvent[], CommandDecisionError>> Decide(CommandDescriptor<InventoryItemCommand> commandDescriptor)
        {

            switch (commandDescriptor.Command.Value)
            {
                case DeactivateInventoryItem _:
                    return Task.FromResult(Ok<InventoryItemEvent[], CommandDecisionError>(new InventoryItemEvent[] {new InventoryItemDeactivated(commandDescriptor.Address)}));
                case CreateInventoryItem createInventoryItem:

                    return Task.FromResult(
                        Ok<InventoryItemEvent[], CommandDecisionError>(
                            new InventoryItemEvent[]
                            {
                                new InventoryItemCreated(createInventoryItem.Name, createInventoryItem.Activated, createInventoryItem.Count, commandDescriptor.Address)
                            }));
                case RenameInventoryItem renameInventoryItem:
                    return Task.FromResult(
                        Ok<InventoryItemEvent[], CommandDecisionError>(new InventoryItemEvent[] {new InventoryItemRenamed(renameInventoryItem.Name, commandDescriptor.Address)}));
                case CheckInItemsToInventory checkInItemsToInventory:
                    return Task.FromResult(
                        Ok<InventoryItemEvent[], CommandDecisionError>(
                            new InventoryItemEvent[] {new ItemsCheckedInToInventory(checkInItemsToInventory.Amount, commandDescriptor.Address)}));
                case RemoveItemsFromInventory removeItemsFromInventory:
                    return Task.FromResult(
                        Ok<InventoryItemEvent[], CommandDecisionError>(
                            new InventoryItemEvent[] {new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, commandDescriptor.Address)}));
                default:
                    throw new NotSupportedException("Unknown command");
            }
        }


        public InventoryItem Apply(params InventoryItemEvent[] events) => events.Aggregate(new InventoryItem(), (_, @event) => @event switch
            {
                InventoryItemCreated inventoryItemCreated => new InventoryItem(inventoryItemCreated.Name, Activated, Count),
                InventoryItemDeactivated _ => new InventoryItem(Name, false, Count),
                ItemsCheckedInToInventory itemsCheckedInToInventory => new InventoryItem(Name, Activated, Count + itemsCheckedInToInventory.Amount),
                ItemsRemovedFromInventory itemsRemovedFromInventory => new InventoryItem(Name, Activated, Count - itemsRemovedFromInventory.Amount),
                InventoryItemRenamed inventoryItemRenamed => new InventoryItem(inventoryItemRenamed.Name, Activated, Count),
                _ => throw new NotSupportedException("Unknown event")
            });
        
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

using System;
using System.Linq;
using System.Threading.Tasks;
using static Radix.Result.Extensions;

namespace Radix.Tests.Models
{
    public class InventoryItem : Aggregate<InventoryItem, InventoryItemEvent, InventoryItemCommand>, IEquatable<InventoryItem>
    {
        public static Update<InventoryItem, InventoryItemEvent> Update = (state, @event) =>
        {
            return @event switch
            {
                InventoryItemCreated inventoryItemCreated => new InventoryItem(inventoryItemCreated.Name, state.Activated, state.Count),
                InventoryItemDeactivated _ => new InventoryItem(state.Name, false, state.Count),
                ItemsCheckedInToInventory itemsCheckedInToInventory => new InventoryItem(state.Name, state.Activated, state.Count + itemsCheckedInToInventory.Amount),
                ItemsRemovedFromInventory itemsRemovedFromInventory => new InventoryItem(state.Name, state.Activated, state.Count - itemsRemovedFromInventory.Amount),
                InventoryItemRenamed inventoryItemRenamed => new InventoryItem(inventoryItemRenamed.Name, state.Activated, state.Count),
                _ => throw new NotSupportedException("Unknown event")
            };
        };

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

        public Task<Result<InventoryItemEvent[], CommandDecisionError>> Decide(TransientCommandDescriptor<InventoryItemCommand> transientCommandDescriptor)
        {
            switch (transientCommandDescriptor.Command.Value)
            {
                case DeactivateInventoryItem _:
                    return Task.FromResult(
                        Ok<InventoryItemEvent[], CommandDecisionError>(new InventoryItemEvent[] {new InventoryItemDeactivated(transientCommandDescriptor.Recipient)}));
                case CreateInventoryItem createInventoryItem:

                    return Task.FromResult(
                        Ok<InventoryItemEvent[], CommandDecisionError>(
                            new InventoryItemEvent[]
                            {
                                new InventoryItemCreated(
                                    createInventoryItem.Name,
                                    createInventoryItem.Activated,
                                    createInventoryItem.Count,
                                    transientCommandDescriptor.Recipient)
                            }));
                case RenameInventoryItem renameInventoryItem:
                    return Task.FromResult(
                        Ok<InventoryItemEvent[], CommandDecisionError>(
                            new InventoryItemEvent[] {new InventoryItemRenamed(renameInventoryItem.Name, transientCommandDescriptor.Recipient)}));
                case CheckInItemsToInventory checkInItemsToInventory:
                    return Task.FromResult(
                        Ok<InventoryItemEvent[], CommandDecisionError>(
                            new InventoryItemEvent[] {new ItemsCheckedInToInventory(checkInItemsToInventory.Amount, transientCommandDescriptor.Recipient)}));
                case RemoveItemsFromInventory removeItemsFromInventory:
                    return Task.FromResult(
                        Ok<InventoryItemEvent[], CommandDecisionError>(
                            new InventoryItemEvent[] {new ItemsRemovedFromInventory(removeItemsFromInventory.Amount, transientCommandDescriptor.Recipient)}));
                default:
                    throw new NotSupportedException("Unknown transientCommand");
            }
        }

        public bool Equals(InventoryItem? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Name == other.Name && Activated == other.Activated && Count == other.Count;
        }


        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((InventoryItem)obj);
        }

        public override int GetHashCode() => HashCode.Combine(Name, Activated, Count);

        public static bool operator ==(InventoryItem? left, InventoryItem? right) => Equals(left, right);

        public static bool operator !=(InventoryItem? left, InventoryItem? right) => !Equals(left, right);
    }
}

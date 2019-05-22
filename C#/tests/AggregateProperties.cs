using System;
using System.Collections.Generic;
using System.Text;
using Radix.Tests.Result;

namespace Radix.Tests
{
    public interface IVersion
    {

    }

    public struct Version : IVersion
    {
        private long Value { get; }

        private Version(long value)
        {
            Value = value;;
        }

        public static implicit operator Version(long value)
        {
            return new Version(value);
        }

        public static implicit operator long(Version version)
        {
            return version.Value;
        }
    }

    public struct Any : IVersion
    {

    }

    public struct Address
    {
    }

    public interface Aggregate<out TState, TEvent, in TCommand>
    {
        TState Identity { get; }

        List<TEvent> Decide(TCommand command);

        TState Apply(TEvent @event);
    }

    public class InventoryItem : Aggregate<InventoryItem, InventoryItemEvent, InventoryItemCommand>
    {
        public string Name { get; }
        public bool Activated { get; }
        public int Count { get; }

        public InventoryItem(string name, bool activated, int count)
        {
            Name = name;
            Activated = activated;
            Count = count;
        }

        public InventoryItem Identity => new InventoryItem("",false,0);

        public List<InventoryItemEvent> Decide(InventoryItemCommand command)
        {
            return command switch
            {
                DeactivateInventoryItem _ => new List<InventoryItemEvent> {new InventoryItemDeactivated()},
                CreateInventoryItem createInventoryItem => new List<InventoryItemEvent> {new InventoryItemCreated(createInventoryItem.Name)},
                RenameInventoryItem renameInventoryItem => new List<InventoryItemEvent> {new InventoryItemRenamed(renameInventoryItem.Name)},
                CheckInItemsToInventory checkInItemsToInventory => new List<InventoryItemEvent> {new ItemsCheckedInToInventory(checkInItemsToInventory.Amount)},
                RemoveItemsFromInventory removeItemsFromInventory => new List<InventoryItemEvent> {new ItemsRemovedFromInventory(removeItemsFromInventory.Amount)},
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
    }

    public interface InventoryItemEvent
    {
    }

    public struct InventoryItemDeactivated : InventoryItemEvent { }

    public struct InventoryItemCreated : InventoryItemEvent
    {
        public string Name { get; }

        public InventoryItemCreated(string name)
        {
            Name = name;

        }
    }

    public struct InventoryItemRenamed : InventoryItemEvent
    {
        public string Name { get; }

        public InventoryItemRenamed(string name)
        {
            Name = name;

        }
    }

    public struct ItemsCheckedInToInventory : InventoryItemEvent
    {
        public int Amount { get; }

        public ItemsCheckedInToInventory(int amount)
        {
            Amount = amount;

        }
    }

    public struct ItemsRemovedFromInventory : InventoryItemEvent
    {
        public int Amount { get; }

        public ItemsRemovedFromInventory(int amount)
        {
            Amount = amount;

        }
    }

    public interface InventoryItemCommand
    {
    }

    public struct DeactivateInventoryItem : InventoryItemCommand {}


    public struct CreateInventoryItem : InventoryItemCommand
    {
        public string Name { get; }

        public CreateInventoryItem(string name)
        {
            Name = name;
        }
    }

    public struct RenameInventoryItem : InventoryItemCommand
    {
        public string Name { get; }

        public RenameInventoryItem(string name)
        {
            Name = name;
        }
    }

    public struct CheckInItemsToInventory : InventoryItemCommand
    {
        public int Amount { get; }

        public CheckInItemsToInventory(int amount)
        {
            Amount = amount;
        }
    }

    public struct RemoveItemsFromInventory : InventoryItemCommand
    {
        public int Amount { get; }

        public RemoveItemsFromInventory(int amount)
        {
            Amount = amount;
        }
    }



    public class BoundedContext
    {
        
    }

    public class AggregateProperties
    {

    }
}

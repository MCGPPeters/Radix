using System;
using System.Collections.Generic;
using System.Linq;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample.Components
{ 

    public class IndexViewModel : State<IndexViewModel, InventoryItemEvent>, IEquatable<IndexViewModel>
    {
        /// <summary>
        /// This is just an example.. in real life this would be a database or something
        /// </summary>
        private static readonly List<(Address address, string Name)> _inventoryItems = new List<(Address, string)>
        {
            (new Address(Guid.NewGuid()), "First item"),
            (new Address(Guid.NewGuid()), "Second item"),
        };

        public IEnumerable<(Address, string)> InventoryItems => _inventoryItems;



        public IndexViewModel Apply(InventoryItemEvent @event)
        {
            (Address address, string Name) item;
            switch (@event)
            {
                case InventoryItemCreated inventoryItemCreated:
                    _inventoryItems.Add((inventoryItemCreated.Address, inventoryItemCreated.Name));
                    break;
                case InventoryItemDeactivated inventoryItemDeactivated:
                    item = _inventoryItems.Find(item => item.address.Equals(inventoryItemDeactivated.Address));
                    _inventoryItems.Remove(item);
                    break;
                case InventoryItemRenamed inventoryItemRenamed:
                    item = _inventoryItems.Find(item => item.address.Equals(inventoryItemRenamed.Address));
                    item.Name = inventoryItemRenamed.Name;
                    break;
            }

            return this;
        }

        public bool Equals(IndexViewModel other)
        {
            return InventoryItems.SequenceEqual(other.InventoryItems);
        }
    }
}

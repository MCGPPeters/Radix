using System;
using System.Collections.Generic;
using System.Linq;
using Radix.Tests.Models;

namespace Radix.Blazor.Inventory.Interface.Logic
{

    public class IndexViewModel : IEquatable<IndexViewModel>
    {
        /// <summary>
        ///     This is just an example.. in real life this would be a database or something
        /// </summary>
        private static readonly List<(Address address, string Name)> _inventoryItems = new List<(Address, string)>();

        public Update<IndexViewModel, InventoryItemEvent> Update =
            (state, @event) =>
            {
                switch (@event)
                {
                    case InventoryItemCreated inventoryItemCreated:
                        state.InventoryItems.Add((inventoryItemCreated.Address, inventoryItemCreated.Name));
                        break;
                    case InventoryItemDeactivated _:
                        (Address address, string Name) itemToDeactivate = state.InventoryItems.Find(item => item.address.Equals(@event.Address));
                        state.InventoryItems.Remove(itemToDeactivate);
                        break;
                    case InventoryItemRenamed inventoryItemRenamed:
                        state.InventoryItems = state.InventoryItems
                            .Select(_ => (@event.Address, inventoryItemRenamed.Name))
                            .Where(tuple => tuple.Address.Equals(@event.Address)).ToList();
                        break;
                    default:
                        throw new NotSupportedException("Unknown event");
                }

                return state;
            };

        public List<(Address address, string name)> InventoryItems
        {
            get => _inventoryItems;
            set => throw new NotImplementedException();
        }

        public bool Equals(IndexViewModel other) => InventoryItems.SequenceEqual(other.InventoryItems);
    }
}

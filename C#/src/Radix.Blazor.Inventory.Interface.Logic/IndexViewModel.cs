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
        private static readonly List<(long id, string Name)> _inventoryItems = new List<(long, string)>();

        public Update<IndexViewModel, InventoryItemEvent> Update =
            (state, @event) =>
            {
                switch (@event)
                {
                    case InventoryItemCreated inventoryItemCreated:
                        state.InventoryItems.Add((inventoryItemCreated.Id, inventoryItemCreated.Name));
                        break;
                    case InventoryItemDeactivated _:
                        (long id, string Name) itemToDeactivate = state.InventoryItems.Find(item => item.address.Equals(@event.Id));
                        state.InventoryItems.Remove(itemToDeactivate);
                        break;
                    case InventoryItemRenamed inventoryItemRenamed:
                        state.InventoryItems = state.InventoryItems
                            .Select(_ => (@event.Id, inventoryItemRenamed.Name))
                            .Where(tuple => tuple.Id.Equals(@event.Id)).ToList();
                        break;
                    default:
                        throw new NotSupportedException("Unknown event");
                }

                return state;
            };

        public List<(long address, string name)> InventoryItems
        {
            get => _inventoryItems;
            set => throw new NotImplementedException();
        }

        public bool Equals(IndexViewModel other) => InventoryItems.SequenceEqual(other.InventoryItems);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Blazor.Inventory.Interface.Logic
{

    public class IndexViewModel : IEquatable<IndexViewModel>, ViewModel
    {
        /// <summary>
        ///     This is just an example.. in real life this would be a database or something
        /// </summary>
        private static readonly List<(Address address, string Name)> _inventoryItems = new List<(Address, string)>();


        public List<(Address address, string name)> InventoryItems
        {
            get => _inventoryItems;
            set => throw new NotImplementedException();
        }

        public bool Equals(IndexViewModel other) => other != null && InventoryItems.SequenceEqual(other.InventoryItems);

        public IEnumerable<Error> Errors { get; set; }
    }
}

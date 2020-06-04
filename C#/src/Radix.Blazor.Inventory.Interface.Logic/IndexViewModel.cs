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

       

        public List<(Address address, string name)> InventoryItems
        {
            get => _inventoryItems;
            set => throw new NotImplementedException();
        }

        public bool Equals(IndexViewModel other) => InventoryItems.SequenceEqual(other.InventoryItems);
    }
}

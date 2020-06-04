using System;
using System.Collections.Generic;

namespace Radix.Blazor.Inventory.Interface.Logic
{
    public class AddInventoryItemViewModel : IEquatable<AddInventoryItemViewModel>
    {
        public string InventoryItemName { get; set; }
        public int InventoryItemCount { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public List<string> Messages { get; set; } = new List<string>();
        public long InventoryItemId { get; set; }

        public bool Equals(AddInventoryItemViewModel other) => true;
    }
}

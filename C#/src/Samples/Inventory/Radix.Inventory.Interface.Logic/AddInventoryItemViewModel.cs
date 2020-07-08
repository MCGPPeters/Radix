using System;
using System.Collections.Generic;
using Radix.Components;

namespace Radix.Blazor.Inventory.Interface.Logic
{
    public class AddInventoryItemViewModel : IEquatable<AddInventoryItemViewModel>, ViewModel
    {
        public string InventoryItemName { get; set; }
        public int InventoryItemCount { get; set; }


        public long InventoryItemId { get; set; }

        public bool Equals(AddInventoryItemViewModel other) => true;

        public IEnumerable<Error> Errors { get; set; }
    }
}

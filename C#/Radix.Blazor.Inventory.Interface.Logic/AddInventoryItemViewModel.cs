using System;
using System.Collections.Generic;
using Radix.Tests.Models;

namespace Radix.Blazor.Inventory.Interface.Logic
{
    public class AddInventoryItemViewModel : IEquatable<AddInventoryItemViewModel>
    {

        public static Update<AddInventoryItemViewModel, InventoryItemEvent> Update = (state, @event) =>
        {
            switch (@event)
            {
                case InventoryItemCreated created:
                    state.Messages.Add($"Created a new item: {created.Name}");
                    state.InventoryItemCount = created.Count;
                    state.InventoryItemName = created.Name;
                    break;
            }

            return state;
        };

        public string InventoryItemName { get; set; }
        public int InventoryItemCount { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public List<string> Messages { get; set; } = new List<string>();

        public bool Equals(AddInventoryItemViewModel other) => true;
    }
}

using System;
using System.Collections.Generic;
using Radix.Tests.Models;

namespace Radix.Blazor.Inventory.Pages
{
    public class AddInventoryItemViewModel : State<AddInventoryItemViewModel, InventoryItemEvent>, IEquatable<AddInventoryItemViewModel>
    {

        public string InventoryItemName { get; set; }
        public int InventoryItemCount { get; set; }

        public List<string> Errors { get; set; } = new List<string>();

        public List<string> Messages { get; set; } = new List<string>();

        public bool Equals(AddInventoryItemViewModel other) => true;

        public AddInventoryItemViewModel Update(params InventoryItemEvent[] @event)
        {
            foreach (InventoryItemEvent inventoryItemEvent in @event)
            {
                switch (inventoryItemEvent)
                {
                    case InventoryItemCreated created:
                        Messages.Add($"Created a new item: {created.Name}");
                        InventoryItemCount = created.Count;
                        InventoryItemName = created.Name;
                        break;
                }
            }

            return this;
        }
    }
}

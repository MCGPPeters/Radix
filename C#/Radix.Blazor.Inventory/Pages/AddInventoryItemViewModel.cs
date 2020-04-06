using Radix.Tests.Models;
using System;
using System.Collections.Generic;

namespace Radix.Blazor.Inventory.Pages
{
    public class AddInventoryItemViewModel : State<AddInventoryItemViewModel, InventoryItemEvent>, IEquatable<AddInventoryItemViewModel>
    {

        public bool Equals(AddInventoryItemViewModel other)
        {
            return true;
        }

        public AddInventoryItemViewModel Apply(params InventoryItemEvent[] @event)
        {
            foreach (var inventoryItemEvent in @event)
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

        public string InventoryItemName { get; set; }
        public int InventoryItemCount { get; set; }

        public List<Error> Errors { get; set; } = new List<Error>();

        public List<string> Messages { get; set; } = new List<string>();
    }
}

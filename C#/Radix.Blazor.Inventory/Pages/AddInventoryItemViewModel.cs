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
            return new AddInventoryItemViewModel();
        }

        public string InventoryItemName { get; set; }
        public int InventoryItemCount { get; set; }
        public IEnumerable<Error> Errors
        {
            get => new List<Error>();
            set => throw new NotImplementedException();
        }
    }
}

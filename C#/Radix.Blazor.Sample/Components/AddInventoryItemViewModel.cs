using System;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample.Components
{
    public class AddInventoryItemViewModel : State<AddInventoryItemViewModel, InventoryItemEvent>, IEquatable<AddInventoryItemViewModel>
    {

        public bool Equals(AddInventoryItemViewModel other)
        {
            return true;
        }

        public AddInventoryItemViewModel Apply(InventoryItemEvent @event)
        {
            return new AddInventoryItemViewModel();
        }
    }
}

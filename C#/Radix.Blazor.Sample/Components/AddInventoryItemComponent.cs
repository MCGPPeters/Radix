using System;
using Radix.Blazor.Html;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample.Components
{
    public class AddInventoryItemComponent : Component<AddInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext, AddInventoryItemViewModel currentViewModel)
        {
            throw new NotImplementedException();
        }
    }

    public class AddInventoryItemViewModel : State<AddInventoryItemViewModel, InventoryItemEvent>, IEquatable<AddInventoryItemViewModel>
    {
        public AddInventoryItemViewModel Apply(InventoryItemEvent @event)
        {
            throw new NotImplementedException();
        }

        public bool Equals(AddInventoryItemViewModel other)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample
{
    public class InventoryItemBoundedContextComponent : BoundedContextComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {

        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext)
        {
            return new Text("Hello");
        }

        public InventoryItemBoundedContextComponent(ReadModel<IndexViewModel, InventoryItemEvent> readModel, BoundedContext<InventoryItemCommand, InventoryItemEvent> context) : base(readModel, context)
        {
        }
    }
}

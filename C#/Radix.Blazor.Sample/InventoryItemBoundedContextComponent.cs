using System;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample
{
    public class InventoryItemBoundedContextComponent : BoundedContextComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        public InventoryItemBoundedContextComponent() : this(new IndexViewModel(), BoundedContext.Create())
        {

        }

        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext)
        {
            return new Text("Hello");
        }

        protected override Node Render()
        {
            return new Text("Hello");
        }

        private InventoryItemBoundedContextComponent(IndexViewModel viewModel, BoundedContext<InventoryItemCommand, InventoryItemEvent> context) : base(viewModel, context)
        {
        }
    }
}

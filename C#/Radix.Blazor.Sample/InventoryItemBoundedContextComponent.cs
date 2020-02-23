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
            throw new NotImplementedException();
        }

        protected override Node Render()
        {
            throw new NotImplementedException();
        }

        private InventoryItemBoundedContextComponent(IndexViewModel viewModel, BoundedContext<InventoryItemCommand, InventoryItemEvent> context) : base(viewModel, context)
        {
        }
    }
}

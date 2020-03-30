using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Blazor.Html;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Components;
using static Radix.Blazor.Html.Elements;

namespace Radix.Blazor.Sample.Components
{
    public class IndexComponent : Component<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        public IndexComponent(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext, ReadModel<IndexViewModel, InventoryItemEvent> readModel,
            IJSRuntime jsRuntime) : base(boundedContext, readModel, jsRuntime)
        {
        }

        public override Node Render(IndexViewModel currentViewModel)
        {
            var InventoryItemNodes = GetInventoryItemNodes(currentViewModel.InventoryItems);

            return concat(
                navLinkMatchAll(new[] {@class("btn btn-primary"), href("Add")}, text("Add")),
                h1(Enumerable.Empty<IAttribute>(), text("All items")),
                ul(Enumerable.Empty<IAttribute>(), InventoryItemNodes)
            );
        }

        private static Node[] GetInventoryItemNodes(IEnumerable<(Address address, string name)> inventoryItems)
        {
            return inventoryItems.Select(
                inventoryItem =>
                    li(
                        Enumerable.Empty<IAttribute>(),
                        navLinkMatchAll(
                            new[] {href($"/Details/{inventoryItem.address}")},
                            text(inventoryItem.name)))).ToArray();
        }
    }
}

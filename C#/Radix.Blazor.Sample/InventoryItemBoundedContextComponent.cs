using Radix.Blazor.Html;
using Radix.Tests.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Components;
using static Radix.Blazor.Html.Elements;

namespace Radix.Blazor.Sample
{
    [Route("/")]
    // ReSharper disable once ClassNeverInstantiated.Global : Instantiated via DI
    public class InventoryItemBoundedContextComponent : BoundedContextComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext, IndexViewModel currentViewModel)
        {
            Node[] InventoryItemNodes = GetInventoryItemNodes(currentViewModel.InventoryItems);

            return concat(
                div(
                    new[] { @class("container") },
                    nav(
                        new[] { @class("navbar navbar-expand-lg navbar-light bg-light") },
                        a(
                            new[] { @class("navbar-brand"), href("#") },
                            text("Inventory"))),

                    div(
                        new[] { @class("row") },
                        nav(
                            new[] { @class("nav") },
                            navLinkMatchAll(new[] { href("/"), @class("nav-link") }, text("Home")),
                            navLinkMatchAll(new[] { href("/Add"), @class("nav-link") }, text("Add")))),

                    div(new[] { @class("row") },
                        ul(Enumerable.Empty<IAttribute>(), InventoryItemNodes))
            ));
        }

        private static Node[] GetInventoryItemNodes(IEnumerable<(Address, string name)> inventoryItems)
        {
            return inventoryItems.Select(inventoryItem =>
                li(Enumerable.Empty<IAttribute>(), text(inventoryItem.name))).ToArray();
        }
    }
}

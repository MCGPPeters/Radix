using System.Collections.Generic;
using System.Linq;
using Radix.Blazor.Html;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Attributes;

namespace Radix.Blazor.Sample
{
    // ReSharper disable once ClassNeverInstantiated.Global : Instantiated via DI
    public class InventoryItemBoundedContextComponent : BoundedContextComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {

        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext)
        {
            return concat(
                div(new[] {@class("container")}, new[]
                {
                    nav(new []{@class("navbar navbar-expand-lg navbar-light bg-light") }, new[]
                    {
                        a(new []{@class("navbar-brand"), href("#")}, new[]
                        {
                            text("Inventory")
                        })
                    })
                }));
        }
    }
}

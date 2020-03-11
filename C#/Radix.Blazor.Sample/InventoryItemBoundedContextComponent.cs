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
                div(
                    new[] {@class("container")},
                    nav(
                        new[] {@class("navbar navbar-expand-lg navbar-light bg-light")},
                        a(
                            new[] {@class("navbar-brand"), href("#")},
                            text("Inventory")))));
        }
    }
}

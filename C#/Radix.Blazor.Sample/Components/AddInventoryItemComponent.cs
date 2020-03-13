using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Components;

namespace Radix.Blazor.Sample.Components
{
    [Route("Add")]
    public class AddInventoryItemComponent : Component<AddInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext, AddInventoryItemViewModel currentViewModel)
        {
            return concat(
                h1(Enumerable.Empty<IAttribute>(), text("Add new item")),
                Elements.form(
                    Enumerable.Empty<IAttribute>(),
                    div(
                        new[] {@class("form-group")},
                        Elements.label(
                            new[] {@for("nameInput")},
                            text("Name")),
                        input(@class("form-control"), id("nameInput"))),
                    navLinkMatchAll(new[] {@class("btn btn-primary"), href("/")}, text("Ok")),
                    navLinkMatchAll(new[] {@class("btn btn-primary"), href("/")}, text("Cancel"))
                ));
        }
    }

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

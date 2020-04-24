using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Monoid;
using Radix.Result;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Components;

namespace Radix.Blazor.Inventory.Components
{
    [Route("/Add")]
    public class AddInventoryItemComponent : Component<AddInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        public override Node View(AddInventoryItemViewModel currentViewModel) => concat(
            h1(NoAttributes(), text("Add new item")),
            div(
                new[] {@class("form-group")},
                Elements.label(
                    new[] {@for("nameInput")},
                    text("Name")),
                input(
                    @class("form-control"),
                    id("nameInput"),
                    bind.input(currentViewModel.InventoryItemName, name => currentViewModel.InventoryItemName = name)),
                Elements.label(
                    new[] {@for("countInput")},
                    text("Count")),
                input(
                    @class("form-control"),
                    id("countInput"),
                    bind.input(currentViewModel.InventoryItemCount, count => currentViewModel.InventoryItemCount = count)
                )),
            button(
                new[]
                {
                    @class("btn btn-primary"), on.click(
                        async args =>
                        {
                            Validated<InventoryItemCommand> validCommand = CreateInventoryItem.Create(
                                currentViewModel.InventoryItemName,
                                true,
                                currentViewModel.InventoryItemCount);

                            // todo creating an aggregate only makes sense when the command is valid
                            Address inventoryItem = await BoundedContext.Create<InventoryItem>();
                            Result<InventoryItemEvent[], Error[]> result = await BoundedContext.Send<InventoryItem>(inventoryItem, validCommand);
                            switch (result)
                            {
                                case Ok<InventoryItemEvent[], Error[]>(var events):
                                    currentViewModel.Update(events);
                                    break;
                                case Error<InventoryItemEvent[], Error[]>(var errors):
                                    currentViewModel.Errors = Enumerable.ToList<string>(errors.Select(error => error.Message));
                                    if (JSRuntime is object)
                                    {
                                        await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                                    }

                                    break;
                            }
                        })
                },
                text("Ok")
            ),
            navLinkMatchAll(new[] {@class("btn btn-primary"), href("/")}, text("Cancel")),
            div(
                NoAttributes(),
                div(
                    new[] {@class("toast"), attribute("data-autohide", "false")},
                    div(
                        new[] {@class("toast-header")},
                        strong(new[] {@class("mr-auto")}, text("Invalid input")),
                        small(NoAttributes(), text(DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture))),
                        button(new[] {type("button"), @class("ml-2 mb-1 close"), attribute("data-dismiss", "toast")}, Elements.span(NoAttributes(), text("🗙")))),
                    div(
                        new[] {@class("toast-body")},
                        FormatErrorMessages(currentViewModel.Errors)
                    ))));

        private static IEnumerable<IAttribute> NoAttributes() => Enumerable.Empty<IAttribute>();

        private static Node FormatErrorMessages(IEnumerable<string> errors)
        {
            Node node = new Empty();
            if (errors is object)
            {
                node = ul(Array.Empty<IAttribute>(), errors.Select(error => li(Array.Empty<IAttribute>(), text(error.ToString()))).ToArray());
            }

            return node;
        }
    }
}

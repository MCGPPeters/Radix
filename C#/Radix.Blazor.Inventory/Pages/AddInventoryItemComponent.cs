using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Monoid;
using Radix.Result;
using Radix.Tests.Models;
using Radix.Validated;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Components;
using static Radix.Blazor.Html.Elements;

namespace Radix.Blazor.Inventory.Pages
{
    [Route("/Add")]
    public class AddInventoryItemComponent : Component<AddInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        public override Node Render(AddInventoryItemViewModel currentViewModel) => concat(
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
                            Address inventoryItem = await BoundedContext.Create<InventoryItem>();
                            Validated<InventoryItemCommand> item = CreateInventoryItem.Create(currentViewModel.InventoryItemName, true, currentViewModel.InventoryItemCount);
                            switch (item)
                            {
                                case Valid<InventoryItemCommand> (var validCommand):
                                    IVersion expectedVersion = new AnyVersion();

                                    Result<InventoryItemEvent[], Error[]> result = await BoundedContext.Send<InventoryItem>(inventoryItem, validCommand, expectedVersion);
                                    switch (result)
                                    {
                                        case Ok<InventoryItemEvent[], Error[]>(var events):
                                            currentViewModel.Apply(events);

                                            break;
                                        case Error<InventoryItemEvent[], Error[]>(var errors):
                                            currentViewModel.Errors = errors.Select(error => error.Message).ToList();
                                            if (JSRuntime is object)
                                            {
                                                await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                                            }

                                            break;
                                    }

                                    break;
                                case Invalid<InventoryItemCommand> (var errors):
                                    currentViewModel.Errors = errors.Select(s1 => s1).ToList();
                                    if (JSRuntime is object)
                                    {
                                        await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                                    }

                                    break;
                                default:
                                    throw new InvalidOperationException();
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
                        small(NoAttributes(), text(DateTimeOffset.UtcNow.ToString())),
                        button(new[] {type("button"), @class("ml-2 mb-1 close"), attribute("data-dismiss", "toast")}, Elements.span(NoAttributes(), text("🗙")))),
                    div(
                        new[] {@class("toast-body")},
                        FormatErrorMessages(currentViewModel.Errors)
                    ))));

        private static IEnumerable<IAttribute> NoAttributes() => Enumerable.Empty<IAttribute>();

        private Node FormatErrorMessages(IEnumerable<string> errors)
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

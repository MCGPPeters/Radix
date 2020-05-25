using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Monoid;
using Radix.Result;
using Radix.Tests.Models;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Components;

namespace Radix.Blazor.Inventory.Server.Pages
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

                            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = await BoundedContext.Create(InventoryItem.Decide, InventoryItem.Update);
                            Result<InventoryItemEvent[], Radix.Error[]> result = await inventoryItem.Accept(validCommand);
                            switch (result)
                            {
                                case Ok<InventoryItemEvent[], Radix.Error[]>(var events):
                                    currentViewModel = events.Aggregate(currentViewModel, (current, @event) => AddInventoryItemViewModel.Update(current, @event));
                                    NavigationManager.NavigateTo("/");
                                    break;
                                case Error<InventoryItemEvent[], Radix.Error[]>(var errors):
                                    currentViewModel.Errors = errors.Select(error => error.Message).ToList();
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

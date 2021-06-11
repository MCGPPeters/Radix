using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Components;
using Radix.Components.Html;
using Radix.Inventory.Domain;
using Radix.Option;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;
using static Radix.Components.Html.Components;

namespace Radix.Blazor.Inventory.Server.Pages
{
    [Route("/Add")]
    public class AddInventoryItemComponent : TaskBasedComponent<AddInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent, Json>
    {
        protected override Node View(AddInventoryItemViewModel currentViewModel) => concat(
            h1(None, text("Add new item")),
            div(
                new[] { @class("form-group") },
                Elements.label(
                    new[] { @for("idInput") },
                    text("Id")),
                input(
                    @class("form-control"),
                    id("idInput"),
                    bind.input(currentViewModel.InventoryItemId, id => currentViewModel.InventoryItemId = id)),
                Elements.label(
                    new[] { @for("nameInput") },
                    text("Name")),
                input(
                    @class("form-control"),
                    id("nameInput"),
                    bind.input(currentViewModel.InventoryItemName, name => currentViewModel.InventoryItemName = name)),
                Elements.label(
                    new[] { @for("countInput") },
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
                                currentViewModel.InventoryItemId,
                                currentViewModel.InventoryItemName,
                                true,
                                currentViewModel.InventoryItemCount);

                            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = BoundedContext.Create(InventoryItem.Decide, InventoryItem.Update);
                            Option<Error[]> result = await Dispatch(inventoryItem, validCommand);
                            switch (result)
                            {
                                case Some<Error[]>(_):
                                    if (JSRuntime is not null)
                                    {
                                        await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                                    }

                                    break;
                                case None<Error[]> _:
                                    NavigationManager.NavigateTo("/");
                                    break;
                            }
                        })
                },
                text("Ok")
            ),
            navLinkMatchAll(new[] { @class("btn btn-primary"), href("/") }, text("Cancel")),
            div(
                None,
                div(
                    new[] { @class("toast"), attribute("data-autohide", "false") },
                    div(
                        new[] { @class("toast-header") },
                        strong(new[] { @class("mr-auto") }, text("Invalid input")),
                        small(None, text(DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture))),
                        button(new[] { type("button"), @class("ml-2 mb-1 close"), attribute("data-dismiss", "toast") }, Elements.span(None, text("🗙")))),
                    div(
                        new[] { @class("toast-body") },
                        FormatErrorMessages(currentViewModel.Errors)
                    ))));

        private static Node FormatErrorMessages(IEnumerable<Error> errors)
        {
            Node node = new Empty();
            if (errors is not null)
            {
                node = ul(Array.Empty<IAttribute>(), errors.Select(error => li(Array.Empty<IAttribute>(), text(error.ToString()))).ToArray());
            }

            return node;
        }
    }
}

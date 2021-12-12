using System.Globalization;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Components;
using Radix.Components.Html;
using Radix.Data;
using Radix.Inventory.Domain;

namespace Radix.Blazor.Inventory.Server.Pages;

[Route("/Add")]
public class AddInventoryItemComponent : TaskBasedComponent<AddInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent, Json>
{
    protected override Node View(AddInventoryItemViewModel currentViewModel) =>
        concat
        (
            h1
            (
                text
                (
                    "Add new item"
                )
            ),
            div
            (
                @class("form-group"),
                label
                (
                    @for("idInput"),
                    text
                    (
                        "Id"
                    )
                ),
                input
                (
                    @class("form-control"),
                    id("idInput"),
                    bind.input(currentViewModel.InventoryItemId, id => currentViewModel.InventoryItemId = id)
                ),
                label
                (
                    @for("nameInput"),
                    text
                    (
                        "Name"
                    )
                ),
                input
                (
                    @class("form-control"),
                    id("nameInput"),
                    bind.input(currentViewModel.InventoryItemName, name => currentViewModel.InventoryItemName = name)
                ),
                label
                (
                    @for("countInput"),
                    text
                    (
                        "Count"
                    )
                ),
                input
                (
                    @class("form-control"),
                    id("countInput"),
                    bind.input(currentViewModel.InventoryItemCount, count => currentViewModel.InventoryItemCount = count)
                )
            ),
            button
            (
                new[]
                {
                        @class("btn btn-primary"),
                        on.click
                        (
                            async args =>
                            {
                                Validated<InventoryItemCommand> validatedCommand = CreateInventoryItem.Create(
                                    currentViewModel.InventoryItemId,
                                    currentViewModel.InventoryItemName,
                                    true,
                                    currentViewModel.InventoryItemCount);

                                var inventoryItem = BoundedContext.Create<InventoryItem, InventoryItemCommandHandler>();
                                Option<Error[]> result = await Dispatch(inventoryItem, validatedCommand);
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
                text
                (
                    "Ok"
                )
            ),
            navLinkMatchAll
            (
                new[]
                {
                    @class("btn btn-secondary"),
                    href("/")
                },
                text
                (
                    "Cancel"
                )
            ),
            div
            (
                div
                (
                    new[]
                    {
                        @class("toast"),
                        attribute("data-autohide", "false")
                    },
                    div
                    (
                        @class("toast-header"),
                        strong
                        (
                            new[]
                            {
                                @class("mr-auto")
                            },
                            text
                            (
                                "Invalid input"
                                )
                            ),
                            small
                            (
                                text
                                (
                                    DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture)
                                )
                            ),
                            button
                            (
                                new[]
                                {
                                    type("button"),
                                    @class("ml-2 mb-1 close"),
                                    attribute("data-dismiss", "toast")
                                },
                                span
                                (
                                    text
                                    (
                                        "🗙"
                                    )
                                )
                            )
                        ),
                        div
                        (
                            @class("toast-body"),
                            FormatErrorMessages(currentViewModel.Errors)
                        )
                    )
                )
            );

    private static Node FormatErrorMessages(IEnumerable<Error> errors)
    {
        Node node = new Empty();
        if (errors is not null)
        {
            node =
                ul
                (
                    errors.Select(error =>
                        li
                        (
                            text
                            (
                                error.ToString()
                            )
                        )
                    ).ToArray()
                );
        }

        return node;
    }
}

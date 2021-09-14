using System.Globalization;
using Microsoft.AspNetCore.Components;
using Radix.Components;
using Radix.Components.Html;
using Radix.Inventory.Domain;
using Radix.Option;

namespace Radix.Blazor.Inventory.Server.Pages;

[Route("/Deactivate/{id:guid}")]
public class DeactivateInventoryItemComponent : TaskBasedComponent<DeactivateInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent, Json>
{
    [Parameter] public Guid Id { get; set; }

    protected override Node View(DeactivateInventoryItemViewModel currentViewModel) =>
        concat
        (
            h1
            (
                text
                (
                    $"Deactivate item : {ViewModel.InventoryItemName}"
                )
            ),
            div
            (
                new[] { @class("form-group") },
                label
                (
                    new[] { @for("reasonInput") },
                    text("Reason")),
                input
                (
                    @class("form-control"),
                    id("reasonInput"),
                    bind.input(currentViewModel.Reason, reason => currentViewModel.Reason = reason)),
                button
                (
                    new[]
                    {
                            @class("btn btn-primary"),
                            on.click(
                                async args =>
                                {
                                    Validated<InventoryItemCommand> validCommand = DeactivateInventoryItem.Create(currentViewModel.Reason);
                                    var inventoryItem = BoundedContext.Get<InventoryItem, InventoryItemCommandHandler>(Id);
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
                div
                (
                    div
                    (
                        new[] { @class("toast"), attribute("data-autohide", "false") },
                        div
                        (
                            new[]
                            {
                                @class("toast-header") },
                                strong
                                (
                                    new[] { @class("mr-auto") },
                                    text("Invalid input")
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
                                        type("button"), @class("ml-2 mb-1 close"),
                                        attribute("data-dismiss", "toast")
                                    },
                                    span
                                    (
                                        text("🗙")
                                    )
                                )
                            ),
                        div
                        (
                            new[]
                            {
                                @class("toast-body")
                            },
                            FormatErrorMessages(currentViewModel.Errors)
                        )
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

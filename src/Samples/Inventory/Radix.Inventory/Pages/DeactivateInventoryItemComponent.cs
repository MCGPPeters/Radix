
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Interaction;
using Radix.Interaction.Components;
using Radix.Interaction.Components.Nodes;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using Radix.Inventory.Domain;
using Radix.Inventory.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using static Radix.Interaction.Web.Components.Components;

namespace Radix.Inventory.Pages;

[Route("/Deactivate/{Id:guid}")]
public class DeactivateInventoryItemComponent : Component<DeactivateInventoryItemModel, Validated<ItemCommand>>
{
    [Parameter] public Guid Id { get; set; }

    [Inject] Context<ItemCommand, ItemEvent, Json> BoundedContext { get; set; } = null!;

    [Inject] NavigationManager NavigationManager { get; init; } = null!;

    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    protected override Interaction.Update<DeactivateInventoryItemModel, Validated<ItemCommand>> Update =>
        async (model, command) =>
        {
            var inventoryItem = BoundedContext.Get<Item, InventoryItemCommandHandler>((Radix.Domain.Data.Aggregate.Id)Id);
            Result<CommandResult<ItemEvent>, Error[]> result = await inventoryItem(command);
            switch (result)
            {
                case Error<CommandResult<ItemEvent>, Error[]>(var errors):
                    model.Errors = errors;
                    if (JSRuntime is not null)
                    {
                        await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                    }

                    break;
                case Ok<CommandResult<ItemEvent>, Error[]>:
                    NavigationManager.NavigateTo("/");
                    break;
            }
            return model;

        };



protected override View<DeactivateInventoryItemModel, Validated<ItemCommand>> View =>
        async (model, dispatch) =>
            concat
            (
                (NodeId)1,
                h1
                (
                    (NodeId)2,
                    text
                    (
                        (NodeId)3,
                        $"Deactivate item : {model.InventoryItemName}"
                    )
                ),
                div
                (
                    (NodeId)4,
                    @class((AttributeId)1, "form-group"),
                    label
                    (
                        (NodeId)5,
                        @for((AttributeId)2, "reasonInput"),
                        text
                        (
                            (NodeId)6,
                            "Reason"
                        )
                    ),
                    input
                    (
                        (NodeId)7,
                        @class((AttributeId)3, "form-control"),
                        id((AttributeId)4, "reasonInput"),
                        bind.input((AttributeId)5, model.Reason, reason => model.Reason = reason)
                    ),
                    button
                    (
                        (NodeId)8,
                        new[]
                        {
                                @class((AttributeId)6, "btn btn-primary"),
                                on.click
                                (
                                    (AttributeId)7,
                                    async args =>
                                    {
                                        Validated<ItemCommand> validCommand = DeactivateItem.Create(model.Reason);
                                        dispatch(validCommand);
                                    }
                                )
                        },
                        text
                        (
                            (NodeId)9,
                            "Ok"
                        )
                    ),
                    navLinkMatchAll
                    (
                        (NodeId)10,
                        new[]
                        {
                            @class((AttributeId)8, "btn btn-secondary"),
                            href((AttributeId)9, "/")
                        },
                        text
                        (
                            (NodeId)50,
                            "Cancel"
                        )
                    ),
                    div
                    (
                        (NodeId)11,
                        div
                        (
                            (NodeId)12,
                            new[]
                            {
                                @class((AttributeId)10, "toast"),
                                attribute((AttributeId)11, "data-autohide", "false")
                            },
                            div
                            (
                                (NodeId)13,
                                @class((AttributeId)12, "toast-header"),
                                strong
                                (
                                    (NodeId)14,
                                    @class((AttributeId)13, "mr-auto"),
                                    text((NodeId)99, "Invalid input")
                                ),
                                small
                                (
                                    (NodeId)15,
                                    text
                                    (
                                        (NodeId)16,
                                        DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture)
                                    )
                                ),
                                button
                                (
                                    (NodeId)17,
                                    new[]
                                    {
                                        type((AttributeId)13, "button"),
                                        @class((AttributeId)14, "ml-2", "mb-1", "close"),
                                        attribute((AttributeId)15, "data-dismiss", "toast")
                                    },
                                    span
                                    (
                                        (NodeId)18,
                                        text
                                        (
                                            (NodeId)19,
                                            "🗙"
                                        )
                                    )
                                )
                            ),
                            div
                            (
                                (NodeId)20,
                                @class((AttributeId)16, "toast-body"),
                                FormatErrorMessages(model.Errors)
                            )
                        )
                    )
                )
            );

    private static Node FormatErrorMessages(IEnumerable<Error> errors)
    {
        Node node = new Empty((NodeId)1);
        if (errors is not null)
        {
            node =
                ul
                (
                    (NodeId)1,
                    errors.Select(error =>
                    li
                    (
                        (NodeId)2,
                        text
                        (
                            (NodeId)3,
                            error.ToString()
                        )
                    )
                ).ToArray()
            );
        }

        return node;
    }
}

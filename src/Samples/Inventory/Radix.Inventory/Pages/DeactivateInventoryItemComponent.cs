
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
                    @class((NodeId)5, "form-group"),
                    label
                    (
                        (NodeId)6,
                        @for((NodeId)7, "reasonInput"),
                        text
                        (
                            (NodeId)8,
                            "Reason"
                        )
                    ),
                    input
                    (
                        (NodeId)9,
                        @class((NodeId)10, "form-control"),
                        id((NodeId)11, "reasonInput"),
                        bind.input((NodeId)12, model.Reason, reason => model.Reason = reason)
                    ),
                    button
                    (
                        (NodeId)13,
                        new[]
                        {
                                @class((NodeId)14, "btn btn-primary"),
                                on.click
                                (
                                    (NodeId)15,
                                    async args =>
                                    {
                                        Validated<ItemCommand> validCommand = DeactivateItem.Create(model.Reason);
                                        dispatch(validCommand);
                                    }
                                )
                        },
                        text
                        (
                            (NodeId)16,
                            "Ok"
                        )
                    ),
                    navLinkMatchAll
                    (
                        (NodeId)17,
                        new[]
                        {
                            @class((NodeId)18, "btn btn-secondary"),
                            href((NodeId)19, "/")
                        },
                        text
                        (
                            (NodeId)20,
                            "Cancel"
                        )
                    ),
                    div
                    (
                        (NodeId)21,
                        div
                        (
                            (NodeId)22,
                            new[]
                            {
                                @class((NodeId)23, "toast"),
                                attribute((NodeId)24, "data-autohide", "false")
                            },
                            div
                            (
                                (NodeId)25,
                                @class((NodeId)26, "toast-header"),
                                strong
                                (
                                    (NodeId)27,
                                    @class((NodeId)28, "mr-auto"),
                                    text((NodeId)29, "Invalid input")
                                ),
                                small
                                (
                                    (NodeId)30,
                                    text
                                    (
                                        (NodeId)31,
                                        DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture)
                                    )
                                ),
                                button
                                (
                                    (NodeId)32,
                                    new[]
                                    {
                                        type((NodeId)33, "button"),
                                        @class((NodeId)34, "ml-2", "mb-1", "close"),
                                        attribute((NodeId)35, "data-dismiss", "toast")
                                    },
                                    span
                                    (
                                        (NodeId)38,
                                        text
                                        (
                                            (NodeId)39,
                                            "🗙"
                                        )
                                    )
                                )
                            ),
                            div
                            (
                                (NodeId)40,
                                @class((NodeId)41, "toast-body"),
                                FormatErrorMessages(model.Errors)
                            )
                        )
                    )
                )
            );

    private static Node FormatErrorMessages(IEnumerable<Error> errors)
    {
        Node node = new Empty((NodeId)42);
        if (errors is not null)
        {
            node =
                ul
                (
                    (NodeId)43,
                    errors.Select(error =>
                    li
                    (
                        (NodeId)44,
                        text
                        (
                            (NodeId)45,
                            error.ToString()
                        )
                    )
                ).ToArray()
            );
        }

        return node;
    }
}

using System.Globalization;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Data;
using Radix.Interaction.Data;
using Radix.Inventory.Domain;
using Radix.Interaction.Components.Nodes;
using Radix.Interaction;
using Radix.Interaction.Components;
using Microsoft.JSInterop;
using Radix.Interaction.Web.Components;
using Radix.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Radix.Inventory.Domain.Data;

namespace Radix.Inventory.Pages;

[Route("/Add")]
public class AddInventoryItemComponent : Component<AddItemModel, Validated<ItemCommand>>
{
    [Inject] Context<ItemCommand, ItemEvent, Json> BoundedContext { get; set; } = null!;

    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    [Inject] NavigationManager NavigationManager { get; init; } = null!;

    protected override Interaction.Update<AddItemModel, Validated<ItemCommand>> Update =>
        async (model, command) =>
        {
            var inventoryItem = BoundedContext.Create<Item, InventoryItemCommandHandler>();
            Result<CommandResult<ItemEvent>, Error[]> result = await inventoryItem(command);
            switch (result)
            {
                case Error<CommandResult<ItemEvent>, Error[]> (var errors):
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

    protected override View<AddItemModel, Validated<ItemCommand>> View =>
        async (model, dispatch) =>
            concat
            (
                (NodeId)101,
                h1
                (
                    (NodeId)1,
                    text
                    (
                        (NodeId)2,
                        "Add new item"
                    )
                ),
                div
                (
                    (NodeId)2,
                    @class((AttributeId)1, "form-group"),
                    label
                    (
                        (NodeId)3,
                        @for((AttributeId)2, "idInput"),
                        text
                        (
                            (NodeId)4,
                            "Id"
                        )
                    ),
                    input
                    (
                        (NodeId)4,
                        @class((AttributeId)4, "form-control"),
                        id((AttributeId)5, "idInput") ,
                        bind.input((AttributeId)6, model.InventoryItemId, id => model.InventoryItemId = id)
                    ),
                    label
                    (
                        (NodeId)5,
                        @for((AttributeId)6, "nameInput"),
                        text
                        (
                            (NodeId)6,
                            "Name"
                        )
                    ),
                    input
                    (
                        (NodeId)7,
                        @class((AttributeId)7, "form-control"),
                        id((AttributeId)8, "nameInput"),
                        bind.input((AttributeId)9, model.InventoryItemName, name => model.InventoryItemName = name)
                    ),
                    label
                    (
                        (NodeId)8,
                        @for((AttributeId)50, "countInput"),
                        text
                        (
                            (NodeId)9,
                            "Count"
                        )
                    ),
                    input
                    (
                        (NodeId)10, 
                        @class((AttributeId)10, "form-control"),
                        id((AttributeId)11, "countInput"),
                        bind.input((AttributeId)12, model.InventoryItemCount, count => model.InventoryItemCount = count)
                    )
                ),
                button
                (
                    (NodeId)11,
                    new[]
                    {
                            @class((AttributeId)13, "btn", "btn-primary"),
                            on.click
                            (
                                (AttributeId)14,
                                args =>
                                {
                                    Validated<ItemCommand> validatedCommand = CreateItem.Create(
                                        model.InventoryItemId,
                                        model.InventoryItemName,
                                        true,
                                        model.InventoryItemCount);

                                    
                                    dispatch(validatedCommand);

                                })
                    },
                    text
                    (
                        (NodeId)12,
                        "Ok"
                    )
                ),
                Components.navLinkMatchAll
                (
                    (NodeId)13,
                    new[]
                    {
                        @class((AttributeId)14, "btn", "btn-secondary"),
                        href((AttributeId)15, "/")
                    },
                    text
                    (
                        (NodeId)14,
                        "Cancel"
                    )
                ),
                div
                (
                    (NodeId)15,
                    div
                    (
                        (NodeId)16,
                        new[]
                        {
                            @class((AttributeId)16, "toast"),
                            attribute((AttributeId)17, "data-autohide", "false")
                        },
                        div
                        (
                            (NodeId)17,
                            @class((AttributeId)18, "toast-header"),
                            strong
                            (
                                (NodeId)18,
                                new[]
                                {
                                    @class((AttributeId)19, "mr-auto")
                                },
                                text
                                (
                                    (NodeId)19,
                                    "Invalid input"
                                )
                            ),
                            small
                            (
                                (NodeId)20,
                                text
                                (
                                    (NodeId)21,
                                    DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture)
                                )
                            ),
                            button
                            (
                                (NodeId)23,
                                new[]
                                {
                                    type((AttributeId)20, "button"),
                                    @class((AttributeId)21, "ml-2", "mb-1", "close"),
                                    attribute((AttributeId)22, "data-dismiss", "toast")
                                },
                                span
                                (
                                    (NodeId)22,
                                    text
                                    (
                                        (NodeId)23,
                                        "🗙"
                                    )
                                )
                            )
                        ),
                        div
                        (
                            (NodeId)24,
                            @class((AttributeId)23, "toast-body"),
                            FormatErrorMessages(model.Errors)
                        )
                    )
                )
            );

    private static Node FormatErrorMessages(IEnumerable<Error> errors)
    {
        Node node = new Empty((NodeId)25);
        if (errors is not null)
        {
            node =
                ul
                (
                    (NodeId)26,
                    errors.Select(error =>
                        li
                        (
                            (NodeId)27,
                            text
                            (
                                (NodeId)28,
                                error.ToString()
                            )
                        )
                    ).ToArray()
                );
        }

        return node;
    }
}

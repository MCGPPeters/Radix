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

    protected override Update<AddItemModel, Validated<ItemCommand>> Update =>
        async (model, command) =>
        {
            var inventoryItem = BoundedContext.Create<Item, ItemCommandHandler>();
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
            await Task.FromResult(concat
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
                    (NodeId)3,
                    @class((NodeId)4, "form-group"),
                    label
                    (
                        (NodeId)5,
                        @for((NodeId)6, "idInput"),
                        text
                        (
                            (NodeId)7,
                            "Id"
                        )
                    ),
                    input
                    (
                        (NodeId)8,
                        @class((NodeId)9, "form-control"),
                        id((NodeId)10, "idInput") ,
                        bind.input((NodeId)11, model.InventoryItemId, id => model.InventoryItemId = id)
                    ),
                    label
                    (
                        (NodeId)12,
                        @for((NodeId)13, "nameInput"),
                        text
                        (
                            (NodeId)14,
                            "Name"
                        )
                    ),
                    input
                    (
                        (NodeId)15,
                        @class((NodeId)16, "form-control"),
                        id((NodeId)17, "nameInput"),
                        bind.input((NodeId)18, model.InventoryItemName, name => model.InventoryItemName = name)
                    ),
                    label
                    (
                        (NodeId)19,
                        @for((NodeId)20, "countInput"),
                        text
                        (
                            (NodeId)21,
                            "Count"
                        )
                    ),
                    input
                    (
                        (NodeId)22, 
                        @class((NodeId)23, "form-control"),
                        id((NodeId)24, "countInput"),
                        bind.input((NodeId)25, model.InventoryItemCount, count => model.InventoryItemCount = count)
                    )
                ),
                button
                (
                    (NodeId)26,
                    new[]
                    {
                            @class((NodeId)27, "btn", "btn-primary"),
                            on.click
                            (
                                (NodeId)28,
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
                        (NodeId)29,
                        "Ok"
                    )
                ),
                 Radix.Interaction.Web.Components.Components.navLinkMatchAll
                (
                    (NodeId)30,
                    new[]
                    {
                        @class((NodeId)31, "btn", "btn-secondary"),
                        href((NodeId)32, "/")
                    },
                    text
                    (
                        (NodeId)33,
                        "Cancel"
                    )
                ),
                div
                (
                    (NodeId)34,
                    div
                    (
                        (NodeId)35,
                        new[]
                        {
                            @class((NodeId)36, "toast"),
                            attribute((NodeId)37, "data-autohide", "false")
                        },
                        div
                        (
                            (NodeId)38,
                            @class((NodeId)39, "toast-header"),
                            strong
                            (
                                (NodeId)40,
                                new[]
                                {
                                    @class((NodeId)41, "mr-auto")
                                },
                                text
                                (
                                    (NodeId)42,
                                    "Invalid input"
                                )
                            ),
                            small
                            (
                                (NodeId)43,
                                text
                                (
                                    (NodeId)44,
                                    DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture)
                                )
                            ),
                            button
                            (
                                (NodeId)45,
                                new[]
                                {
                                    type((NodeId)46, "button"),
                                    @class((NodeId)47, "ml-2", "mb-1", "close"),
                                    attribute((NodeId)48, "data-dismiss", "toast")
                                },
                                span
                                (
                                    (NodeId)49,
                                    text
                                    (
                                        (NodeId)50,
                                        "🗙"
                                    )
                                )
                            )
                        ),
                        div
                        (
                            (NodeId)51,
                            @class((NodeId)52, "toast-body"),
                            FormatErrorMessages(model.Errors)
                        )
                    )
                )
            ));

    private static Node FormatErrorMessages(IEnumerable<Error>? errors)
    {
        Node node = new Empty((NodeId)53);
        if (errors is not null)
        {
            node =
                ul
                (
                    (NodeId)54,
                    errors.Select(error =>
                        li
                        (
                            (NodeId)55,
                            text
                            (
                                (NodeId)56,
                                error.ToString()
                            )
                        )
                    ).ToArray()
                );
        }

        return node;
    }
}

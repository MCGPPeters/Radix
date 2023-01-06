using System.Buffers;
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
using Attribute = Radix.Interaction.Data.Attribute;

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

    protected override View<AddItemModel, Validated<ItemCommand>> View =>
        async (model, dispatch) =>
            await Task.FromResult(
                concat
                (
                    new Node[]
                    {
                        h1
                        (
                            Array.Empty<Attribute>(),
                            new Node[]
                            {
                                text
                                (
                                    "Add new item"
                                )
                            }

                        ),
                        div
                        (
                            new Attribute[] {@class((NodeId)4, "form-group")},
                            new Node[]
                            {
                                label
                                (
                                    new Attribute[] {@for((NodeId)6, "idInput"),},
                                    new[]
                                    {
                                        text
                                        (
                                            "Id"
                                        )
                                    }
                                ),
                                input
                                (
                                    new[]
                                    {
                                        @class((NodeId)9, "form-control"), id((NodeId)10, "idInput"), bind.input(
                                            (NodeId)11, model.InventoryItemId,
                                            id => model.InventoryItemId = id)
                                    },
                                    Array.Empty<Node>()
                                ),
                                label
                                (
                                    new Attribute[] {@for((NodeId)13, "nameInput")},
                                    new[]
                                    {
                                        text
                                        (
                                            "Name"
                                        )
                                    }

                                ),
                                input
                                (
                                    new[]
                                    {
                                        @class((NodeId)16, "form-control"), id((NodeId)17, "nameInput"), bind.input(
                                            (NodeId)18, model.InventoryItemName,
                                            name => model.InventoryItemName = name)
                                    },
                                    Array.Empty<Node>()
                                ),
                                label
                                (
                                    new Attribute[] {@for((NodeId)20, "countInput")},
                                    new[] {text("Count")}
                                ),
                                input
                                (
                                    new Attribute[]
                                    {
                                        @class((NodeId)23, "form-control"), id((NodeId)24, "countInput"), bind.input(
                                            (NodeId)25, model.InventoryItemCount,
                                            count => model.InventoryItemCount = count)
                                    },
                                    Array.Empty<Node>()
                                )
                            }
                        ),
                        button
                        (
                            new[]
                            {
                                @class((NodeId)27, "btn", "btn-primary"), on.click
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
                            new[]
                            {
                                text
                                (
                                    "Ok"
                                )
                            }
                        ),
                        Radix.Interaction.Web.Components.Components.navLinkMatchAll
                        (
                            new[] {@class((NodeId)31, "btn", "btn-secondary"), href((NodeId)32, "/")},
                            new[]
                            {
                                text
                                (
                                    "Cancel"
                                )
                            }
                        ),
                        div
                        (
                            Array.Empty<Attribute>(),
                            new Node[]
                            {
                                div
                                (
                                    new[]
                                    {
                                        @class((NodeId)36, "toast"), attribute((NodeId)37, "data-autohide", "false")
                                    },
                                    new Node[]
                                    {
                                        div
                                        (
                                            new Attribute[] {@class((NodeId)39, "toast-header")},
                                            new Node[]
                                            {
                                                strong
                                                (
                                                    new Attribute[] {@class((NodeId)41, "mr-auto")},
                                                    new[]
                                                    {
                                                        text
                                                        (
                                                            "Invalid input"
                                                        )
                                                    }
                                                ),
                                                small
                                                (
                                                    Array.Empty<Attribute>(),
                                                    new[]
                                                    {
                                                        text
                                                        (
                                                            DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture)
                                                        )
                                                    }
                                                ),
                                                button
                                                (
                                                    new[]
                                                    {
                                                        type((NodeId)46, "button"),
                                                        @class((NodeId)47, "ml-2", "mb-1", "close"),
                                                        attribute((NodeId)48, "data-dismiss", "toast")
                                                    },
                                                    new Node[]
                                                    {
                                                        span
                                                        (
                                                            Array.Empty<Attribute>(),
                                                            new[]
                                                            {
                                                                text
                                                                (
                                                                    "🗙"
                                                                )
                                                            }

                                                        )
                                                    }
                                                )
                                            }
                                        ),
                                        div
                                        (
                                            new Attribute[] {@class((NodeId)52, "toast-body")},
                                            new[] {FormatErrorMessages(model.Errors)}
                                        )
                                    })
                            }
                        )
                    }));

    private static Node FormatErrorMessages(IEnumerable<Error>? errors)
    {
        Node node = new Empty();
        if (errors is not null)
        {
            node =
                ul
                (
            Array.Empty<Attribute>(),
                    errors.Select(error =>
                        (Node)li
                        (
                            Array.Empty<Attribute>(),
                            new[]
                            {
                            text
                            (
                                error.ToString()
                            )
                            }

                        )
                    ).ToArray()
                );
        }

        return node;
    }
}

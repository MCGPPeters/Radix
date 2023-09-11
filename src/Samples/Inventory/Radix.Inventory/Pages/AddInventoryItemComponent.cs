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
using Radix.Tests;
using Attribute = Radix.Interaction.Data.Attribute;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Radix.Inventory.Pages;

[Route("/Add")]
public class AddInventoryItemComponent : Component<AddItemModel, Validated<InventoryCommand>>
{
    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    [Inject] NavigationManager NavigationManager { get; init; } = null!;


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

    protected override Node View(AddItemModel model, Action<Validated<InventoryCommand>> dispatch) =>
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
                            new Attribute[] { @class(new[] { "form-group" }) },
                            new Node[]
                            {
                                label
                                (
                                    new Attribute[] { @for(new[] { "idInput" }), },
                                    new[]
                                    {
                                        text
                                        (
                                            "Address"
                                        )
                                    }
                                ),
                                input
                                (
                                    new[]
                                    {
                                        @class(new[] { "form-control" }),
                                        id(new[] { "idInput" }),
                                        bind.input(
                                            model.InventoryItemId,
                                            id => model.InventoryItemId = id)
                                    },
                                    Array.Empty<Node>()
                                ),
                                label
                                (
                                    new Attribute[] { @for(new[] { "nameInput" }) },
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
                                        @class(new[] { "form-control" }), id(new[] { "nameInput" }), bind.input(
                                            model.InventoryItemName,
                                            name => model.InventoryItemName = name)
                                    },
                                    Array.Empty<Node>()
                                ),
                                label
                                (
                                    new Attribute[] { @for(new[] { "countInput" }) },
                                    new[] { text("Count") }
                                ),
                                input
                                (
                                    new Attribute[]
                                    {
                                        @class(new[] { "form-control" }), id(new[] { "countInput" }), bind.input(
                                            model.InventoryItemCount,
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
                                @class(new[] { "btn", "btn-primary" }),
                                on.click
                                (
                                    args =>
                                    {
                                        Validated<InventoryCommand> validatedCommand = CreateItem.Create(
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
                            new[]
                            {
                                @class(new[] { "btn", "btn-secondary" }),
                                href(new[] { "/" })
                            },
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
                                        @class(new[] { "toast" }), attribute("data-autohide", new[] { "false" })
                                    },
                                    new Node[]
                                    {
                                        div
                                        (
                                            new Attribute[] { @class(new[] { "toast-header" }) },
                                            new Node[]
                                            {
                                                strong
                                                (
                                                    new Attribute[] { @class(new[] { "mr-auto" }) },
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
                                                        type(new[] { "button" }),
                                                        @class(new[] { "ml-2", "mb-1", "close" }),
                                                        attribute("data-dismiss", new[] { "toast" })
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
                                            new Attribute[] { @class(new[] { "toast-body" }) },
                                            new[] { FormatErrorMessages(model.Errors) }
                                        )
                                    })
                            }
                        )
                    });
    protected override async ValueTask<AddItemModel> Update(AddItemModel model, Validated<InventoryCommand> command)
    {
        Context<InventoryCommand, InventoryEvent, InMemoryEventStore, InMemoryEventStoreSettings> context = new() { EventStoreSettings = new InMemoryEventStoreSettings() };

        var inventoryItem = await context.Create<Item>();

        try
        {
            var result = await inventoryItem.Handle(command);
        }
        catch (ValidationErrorException e)
        {
            model.Errors = e.Reasons.Select(r => new Error { Message = r.ToString() });
            await JSRuntime.InvokeAsync<string>("toast", []);
        }

        NavigationManager.NavigateTo("/");

        return model;
    }
}

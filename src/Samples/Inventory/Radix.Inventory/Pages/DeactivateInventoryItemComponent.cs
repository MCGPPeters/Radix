﻿
using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Domain.Data.Aggregate;
using Radix.Interaction;
using Radix.Interaction.Components;
using Radix.Interaction.Components.Nodes;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using Radix.Inventory.Domain;
using Radix.Inventory.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Radix.Tests;
using static Radix.Interaction.Web.Components.Components;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Inventory.Pages;

[Route("/Deactivate/{Id:guid}")]
public class DeactivateInventoryItemComponent : Component<DeactivateInventoryItemModel, Validated<InventoryCommand>>
{
    [Parameter] public Guid Id { get; set; }

    [Inject] NavigationManager NavigationManager { get; init; } = null!;

    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    protected override Node View(DeactivateInventoryItemModel model, Action<Validated<InventoryCommand>> dispatch) =>
        concat
                (
                    new Node[]
                    {
                    h1
                    (
                        Array.Empty<Attribute>(),
                        new []
                        {
                            text
                            (
                                $"Deactivate item : {model.InventoryItemName}"
                            )
                        }

                    ),
                    div
                    (
                        new Attribute[]
                        {
                            @class(new []{"form-group"})
                        },
                        new Node[]
                        {
                            label
                            (
                                new Attribute[]
                                {
                                    @for(new []{"reasonInput"})
                                },
                                new []
                                {
                                    text
                                    (
                                        "Reason"
                                    )
                                }

                            ),
                            input
                            (
                                new []
                                {
                                    @class(new []{"form-control"}),
                                    id(new []{"reasonInput"}),
                                    bind.input(model.Reason, reason => model.Reason = reason)
                                },
                                Array.Empty<Node>()
                            ),
                            button
                            (
                                new[]
                                {
                                        @class(new []{ "btn btn-primary"}),
                                        on.click
                                        (
                                            _ =>
                                            {
                                                Validated<InventoryCommand> validCommand = DeactivateItem.Create(model.Reason);
                                                dispatch(validCommand);
                                            }
                                        )
                                },
                                new []
                                {
                                    text
                                    (
                                        "Ok"
                                    )
                                }

                            ),
                            navLinkMatchAll
                            (
                                new[]
                                {
                                    @class(new []{"btn btn-secondary"}),
                                    href(new []{ "/"})
                                },
                                new []
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
                                            @class(new []{"toast"}),
                                            attribute("data-autohide", new []{"false"})
                                        },
                                        new Node[]
                                        {
                                            div
                                            (
                                                new Attribute[]
                                                {
                                                    @class(new []{ "toast-header"})
                                                },
                                                new Node[]
                                                {
                                                    strong
                                                    (
                                                        new Attribute[]
                                                        {
                                                            @class(new []{"mr-auto"})
                                                        },
                                                        new []
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
                                                        new []
                                                        {
                                                            text
                                                            (
                                                                DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture)
                                                            )
                                                        }

                                                    ),
                                                    button
                                                    (
                                                        new Attribute[]
                                                        {
                                                            type(new[] { "button" }),
                                                            @class(new[]{"ml-2", "mb-1", "close"}),
                                                            attribute("data-dismiss", new []{"toast"})

                                                        },
                                                        new Node[]
                                                        {
                                                            span
                                                            (
                                                                Array.Empty<Attribute>(),
                                                                new []
                                                                {
                                                                    text
                                                                    (
                                                                        "🗙"
                                                                    )
                                                                }
                                                            )
                                                        })


                                                }

                                            )
                                        })
                                    ,
                                    div
                                    (
                                        new Attribute[]
                                        {
                                            @class(new[] { "toast-body" })
                                        },
                                        new Node[]
                                        {
                                            FormatErrorMessages(model.Errors)
                                        }

                                    )

                                }


                            )

                        }
                    )
                        }
                );
    

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
                        new []
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


    protected override async ValueTask<DeactivateInventoryItemModel> Update(DeactivateInventoryItemModel model, Validated<InventoryCommand> command)
    {
        Context<InventoryCommand, InventoryEvent, InMemoryEventStore, InMemoryEventStoreSettings> context = new() { EventStoreSettings = new InMemoryEventStoreSettings() };

        // for testing purposes make the aggregate block the current thread while processing
        var inventoryItem = await context.Get<Item>(new Address() { Id = (Radix.Domain.Data.Aggregate.Id)Id });
        var result = await inventoryItem.Handle(command);

        async void HandleReasons(Reason[] reasons)
        {
            model.Errors = reasons.Select(reason => new Error() { Message = reason.ToString() });
            await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
        }

        result
            .Match(_ =>
            {
                NavigationManager.NavigateTo("/");
            }, HandleReasons);

        return model;

    }
}

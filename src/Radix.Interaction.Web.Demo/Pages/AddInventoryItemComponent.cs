﻿using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Inventory.Domain;
using Radix.Inventory.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;
using Attribute = Radix.Interaction.Data.Attribute;
using script = Radix.Web.Html.Data.Names.Elements.script;

namespace Radix.Interaction.Web.Demo.Pages;

[Route("/Add")]
[RenderModeServer]
public class AddItem : Component<AddItemModel, Validated<InventoryCommand>>
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
        section
        (
            [],
            [
                h1
                (
                    [],
                    [
                        "Add new item"
                    ]
                ),
                div
                (
                    [
                        @class(["form-group"])
                    ],
                    [
                        label
                        (
                            [
                                @for(["idInput"])
                            ],
                            [
                                "Id"
                            ]
                        ),
                        input
                        (
                            [
                                @class(["form-control"]),   
                                id(["idInput"]),
                                bind.input(model.InventoryItemId, id => model.InventoryItemId = id)
                            ],
                            []
                        ),
                        label
                        (
                            [
                                @for(["nameInput"])
                            ],
                            [
                                "Name"
                            ]
                        ),
                        input
                        (
                            [
                                @class(["form-control"]),
                                id(["nameInput"]),
                                bind.input(model.InventoryItemName, name => model.InventoryItemName = name)
                            ],
                            []
                        ),
                        label
                        (
                            [
                                @for(["countInput"])
                            ],
                            [
                                "Count"
                            ]
                        ),
                        input
                        (
                            [
                                @class(["form-control"]),
                                id(["countInput"]),
                                bind.input(model.InventoryItemCount, count => model.InventoryItemCount = count)
                            ]
                            ,
                            []
                        )
                    ]
                ),
                button
                (
                    [
                        @class(["btn", "btn-primary"]),
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
                    ],
                    [
                        "Ok"
                    ]
                ),
                navLinkMatchAll
                (
                    [
                        @class(["btn", "btn-secondary"]),
                        href(["/"])
                    ],
                    [
                        "Cancel"
                    ]
                ),
                div
                (
                    [
                        @class(["toast-container"])
                    ],
                    [
                        div
                        (
                            [
                                @class(["toast"]),
                                id(["errors"]),
                                attribute("role", ["alert"]),
                                attribute("aria-live", ["assertive"]),
                                attribute("aria-atomic", ["true"]),
                                attribute("data-bs-autohide", ["false"])
                            ],
                            [
                                div
                                (
                                    [
                                        @class(["toast-header"])
                                    ],
                                    [
                                        strong
                                        (
                                            [
                                                @class(["me-auto"])
                                            ],
                                            [
                                                "Invalid input"
                                            ]
                                        ),
                                        small
                                        (
                                            [],
                                            [
                                                DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture)
                                            ]
                                        ),
                                        button
                                        (
                                            [
                                                type(["button"]),
                                                @class(["btn-close"]),
                                                attribute("data-bs-dismiss",["toast"]),
                                                attribute("aria-label",["Close"])
                                            ],
                                            [
                                            ]
                                        )
                                    ]
                                ),
                                div
                                (
                                    [
                                        @class(["toast-body"])
                                    ],
                                    [
                                        FormatErrorMessages(model.Errors)
                                    ]
                                )
                            ]
                        )
                    ]
                )

            ]
        );

    protected override async ValueTask<AddItemModel> Update(AddItemModel model, Validated<InventoryCommand> command)
    {
        Context<InventoryCommand, InventoryEvent, InMemoryEventStore, InMemoryEventStoreSettings> context = new() { EventStoreSettings = new InMemoryEventStoreSettings() };

        // for testing purposes make the aggregate block the current thread while processing
        var item = await context.Create<Item>();
        var result = await item.Handle(command);

        switch (result)
        {
            case Valid<Instance<Item, InventoryCommand, InventoryEvent>>:
                NavigationManager.NavigateTo("/");
                break;
            case Invalid<Instance<Item, InventoryCommand, InventoryEvent>>(var reasons):
                model.Errors = reasons.Select(r => new Error { Message = r.ToString() });
                await JSRuntime.InvokeAsync<string>("toast", []);
                break;
        }

        return model;
    }
}

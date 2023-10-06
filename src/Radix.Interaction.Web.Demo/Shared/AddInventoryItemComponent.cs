using System.Globalization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Data;
using Radix.Domain.Data;
using Radix.Inventory.Domain;
using Radix.Inventory.Domain.Data;
using Radix.Inventory.Domain.Data.Commands;
using Radix.Inventory.Domain.Data.Events;

namespace Radix.Interaction.Web.Demo.Shared;

public class AddInventoryItem : Component<AddItemModel, Validated<InventoryCommand>>
{
    [Inject] IJSRuntime JSRuntime { get; init; } = null!;

    [Inject] NavigationManager NavigationManager { get; init; } = null!;

    private static Node FormatErrorMessages(IEnumerable<Error>? errors)
    {
        Node node = empty();
        if (errors is not null)
        {
            node =
                ul
                (
                    [],
                    errors.Select(error =>
                        li
                        (
                            [],
                            [
                                text(error.ToString())
                            ]
                        )
                    ).ToArray()
                );
        }

        return node;
    }

    public override Node[] View(AddItemModel model, Func<Validated<InventoryCommand>, Task> dispatch) =>
        [
            section
        (
            [
                h1
                (
                    [
                        text("Add new item")
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
                                text("Id")
                            ]
                        ),
                        input
                        (
                            [
                                @class(["form-control"]),
                                id(["idInput"]),
                                bind.input(model.InventoryItemId, id => Model = model with { InventoryItemId = id })
                                ],

                                []
                        ),
                        label
                        (
                            [
                                @for(["nameInput"])
                            ],
                            [
                                text("Name")
                            ]
                        ),
                        input
                        (
                            [
                                @class(["form-control"]),
                                id(["nameInput"]),
                                bind.input(model.InventoryItemName, name => Model = Model with { InventoryItemName = name })
                            ],
                            []
                        ),
                        label
                        (
                            [
                                @for(["countInput"])
                            ],
                            [
                                text("Count")
                            ]
                        ),
                        input
                        (
                            [
                                @class(["form-control"]),
                                id(["countInput"]),
                                bind.input(model.InventoryItemCount, count => Model = Model with
                                {
                                    InventoryItemCount = count
                                })
                            ],
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
                        text("Ok")
                    ]
                ),
                navLinkMatchAll
                (
                    [
                        @class(["btn", "btn-secondary"]),
                        href(["/inventory"])
                    ],
                    [
                        text("Cancel")
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
                                                text("Invalid input")
                                            ]
                                        ),
                                        small
                                        (
                                            [],
                                            [
                                                text(DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture))
                                            ]
                                        ),
                                        button
                                        (
                                            [
                                                type(["button"]),
                                                @class(["btn-close"]),
                                                attribute("data-bs-dismiss", ["toast"]),
                                                attribute("aria-label", ["Close"])
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
        )];

    public override async ValueTask<AddItemModel> Update(AddItemModel model, Validated<InventoryCommand> command)
    {
        Context<InventoryCommand, InventoryEvent, InMemoryEventStore, InMemoryEventStoreSettings> context = new() { EventStoreSettings = new InMemoryEventStoreSettings() };

        // for testing purposes make the aggregate block the current thread while processing
        var item = await context.Create<Item>();
        var result = await item.Handle(command);

        switch (result)
        {
            case Valid<Instance<Item, InventoryCommand, InventoryEvent>>:
                NavigationManager.NavigateTo("/inventory");
                break;
            case Invalid<Instance<Item, InventoryCommand, InventoryEvent>>(var reasons):
                model = model with { Errors = reasons.Select(r => new Error { Message = r.ToString() }) };
                await JSRuntime.InvokeAsync<string>("toast", []);
                break;
        }

        return model;
    }
}

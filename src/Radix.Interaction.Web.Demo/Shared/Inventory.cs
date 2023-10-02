using Radix.Data;
using Radix.Inventory.Domain.Data.Commands;
using Microsoft.AspNetCore.Components;

namespace Radix.Interaction.Web.Demo.Shared;

public class Inventory : Component<InventoryModel, ItemCommand>
{
    public override async ValueTask<InventoryModel> Update(InventoryModel model, ItemCommand command) => model;


    public override Node[] View(InventoryModel model, Func<ItemCommand, Task> dispatch) =>
        [
            section
            (
                [],
                [
                    navLinkMatchAll
                    (
                        [
                            @class(["btn", "btn-primary"]),
                            href(["Add"])
                        ],
                        [
                            text
                            (
                                "Add"
                            )
                        ]
                    ),
                    h1
                    (
                        [],
                        [
                            text
                            (
                                "All items"
                            )
                        ]
                    ),
                    table
                    (
                        [],
                        GetInventoryItemNodes(model.Items)
                    )
                ]
            )
        ];

    private static Node[] GetInventoryItemNodes(IEnumerable<ItemModel>? inventoryItems) =>
        inventoryItems?
            .Select(static inventoryItem =>
                tr
                (
                    [],
                    [
                        td
                        (
                            [],
                            [
                                navLinkMatchAll
                                (
                                    [
                                        href([$"/Details/{inventoryItem.Id}"])
                                    ],
                                    [
                                        text
                                        (
                                            inventoryItem.Name ?? string.Empty
                                        )
                                    ]
                                )
                            ]
                        ),
                        // conditional output
                        inventoryItem.Activated
                    ?
                        td
                        (
                            [],
                            [
                                navLinkMatchAll
                                (

                                    [
                                        href([$"/Deactivate/{inventoryItem.Id}"])
                                    ],
                                    [
                                        text
                                        (
                                            "Deactivate"
                                        )
                                    ]
                                )
                            ]
                        )
                    :
                        td
                        (
                            [],
                            [
                                navLinkMatchAll
                                (
                                    [
                                        href([$"/Activate/{inventoryItem.Id}"])
                                    ],
                                    [
                                        text
                                        (
                                            "Activate"
                                        )
                                    ]
                                )
                            ]


                        )
                    ]
                )
            ).ToArray()
        ??
           [];
}

public class InventoryModel
{
    public required List<ItemModel> Items { get; set; }
}
public record ItemModel
{
    public required Domain.Data.Aggregate.Address? Id { get; set; }
    public required string Name { get; set; }
    public required bool Activated { get; set; }
}

﻿using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using static Radix.Interaction.Web.Components.Components;
using Radix.Data;
using Radix.Interaction.Web.Components;
using Radix.Interaction;
using Radix.Interaction.Data;
using Node = Radix.Interaction.Data.Node;
using Radix.Inventory.Domain.Data.Commands;
using Attribute = Radix.Interaction.Data.Attribute;

namespace Radix.Inventory.Pages;

[Route("/")]
public class IndexComponent : Component<IndexModel, Validated<ItemCommand>>
{
    protected override Update<IndexModel, Validated<ItemCommand>> Update => (model, _) => Task.FromResult(model);

    protected override View<IndexModel, Validated<ItemCommand>> View =>
         static (model, _) =>
            Task.FromResult<Node>(
            concat
            (
                new Node[]
                {
                    navLinkMatchAll
                    (
                        new Attribute[]
                        {
                            @class(new []{ "btn", "btn-primary"}),
                            href(new[] { "Add" })
                        },
                        new[]
                        {
                            text
                            (
                                "Add"
                            )
                        }
                    ),
                    h1
                    (
                        Array.Empty<Attribute>(),
                        new []
                        {
                            text
                            (
                                "All items"
                            )
                        }
                    ),
                    table
                    (
                        Array.Empty<Attribute>(),
                        GetInventoryItemNodes(model.InventoryItems)
                    )
                })
            );


    private static Node[] GetInventoryItemNodes(IEnumerable<ItemModel>? inventoryItems) =>
        inventoryItems?
            .Select(static inventoryItem =>
                (Node)tr
                (
                    Array.Empty<Attribute>(),
                    new Node[]
                    {
                        td
                    (
                        Array.Empty<Attribute>(),
                        new Node[]
                        {
                            navLinkMatchAll
                            (
                                new Attribute[]
                                {
                                    href(new[] { $"/Details/{inventoryItem.Id}" })
                                },
                                new[]
                                {
                                    text
                                    (
                                        inventoryItem.Name ?? string.Empty
                                    )
                                }

                            )
                        }

                    ),
                    // conditional output
                    inventoryItem.Activated
                    ?
                        td
                        (
                            Array.Empty<Attribute>(),
                            new Node[]
                            {
                                navLinkMatchAll
                                (

                                    new Attribute[]
                                    {
                                        href(new[] { $"/Deactivate/{inventoryItem.Id}" })
                                    },

                                    new Node[]
                                    {
                                        text
                                        (
                                            "Deactivate"
                                        )
                                    }
                                )
                            }

                        )
                    :
                        td
                        (
                            Array.Empty<Attribute>(),
                            new Node[]
                            {
                                navLinkMatchAll
                                (
                                    new Attribute[]
                                    {
                                        href(new[] { $"/Activate/{inventoryItem.Id}" })
                                    },
                                    new Node[]
                                    {
                                        text
                                        (
                                            "Activate"
                                        )
                                    }
                                )
                            }


                        )
                    })
            ).ToArray()
        ??
            Array.Empty<Node>();

}
                    

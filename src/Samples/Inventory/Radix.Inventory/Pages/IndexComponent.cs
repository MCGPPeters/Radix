using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using static Radix.Interaction.Web.Components.Components;
using Radix.Data;
using Radix.Interaction.Web.Components;
using Radix.Interaction;
using Radix.Interaction.Data;
using Node = Radix.Interaction.Data.Node;
using Radix.Inventory.Domain.Data.Commands;

namespace Radix.Inventory.Pages;

[Route("/")]
public class IndexComponent : Component<IndexModel, Validated<ItemCommand>>
{
    protected override Update<IndexModel, Validated<ItemCommand>> Update => (model, _) => Task.FromResult(model);

    protected override View<IndexModel, Validated<ItemCommand>> View =>
         static (model, dispatch) =>
            Task.FromResult<Node>(concat
            (
                (NodeId)1,
                navLinkMatchAll
                (
                    (NodeId)2,
                    new[]
                    {
                        @class((NodeId)3, "btn", "btn-primary"),
                        href((NodeId)4, "Add")
                    },
                    text
                    (
                        (NodeId)5,
                        "Add"
                    )
                ),
                h1
                (
                    (NodeId)6,
                    text
                    (
                        (NodeId)7,
                        "All items"
                    )
                ),
                table
                (
                    (NodeId)8,
                    GetInventoryItemNodes(model.InventoryItems)
                )
            ));   


    private static Node[] GetInventoryItemNodes(IEnumerable<ItemModel>? inventoryItems) =>
        inventoryItems?
            .Select(static inventoryItem =>
                tr
                (
                    (NodeId)10,
                    td
                    (
                        (NodeId)8,
                        navLinkMatchAll
                        (
                            (NodeId)11,      
                            new[]
                            {
                                href((NodeId)12, $"/Details/{inventoryItem.Id}")
                            },
                            text
                            (
                                (NodeId)13,
                                inventoryItem.Name ?? string.Empty
                            )
                        )
                    ),
                    // conditional output
                    inventoryItem.Activated
                    ?
                        td
                        (
                            (NodeId)14,
                            navLinkMatchAll
                            (
                                (NodeId)15,
                                new[]
                                {
                                    href((NodeId)16, $"/Deactivate/{inventoryItem.Id}")
                                },
                                text
                                (
                                    (NodeId)17,
                                    "Deactivate"
                                )
                            )
                        )
                    :
                        td
                        (
                            (NodeId)14,
                            navLinkMatchAll
                            (
                                (NodeId)18,
                                new[]
                                {
                                    href((NodeId)19, $"/Activate/{inventoryItem.Id}")
                                },
                                text
                                (
                                    (NodeId)20,
                                    "Activate"
                                )
                            )
                        )
                    )
            ).ToArray()
        ??
            Array.Empty<Node>();
}

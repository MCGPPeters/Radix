using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using static Radix.Interaction.Web.Components.Components;

using Radix.Inventory.Domain;
using Radix.Data;
using Radix.Interaction.Web.Components;
using Radix.Interaction;
using Radix.Interaction.Data;
using Node = Radix.Interaction.Data.Node;

namespace Radix.Inventory.Pages;

[Route("/")]
public class IndexComponent : Component<IndexModel, Validated<InventoryItemCommand>>
{
    protected override Interaction.Update<IndexModel, Validated<InventoryItemCommand>> Update => (model, _) => Task.FromResult(model);

    protected override View<IndexModel, Validated<InventoryItemCommand>> View =>
        async (model, dispatch) =>
            concat
            (
                (NodeId)1,
                navLinkMatchAll
                (
                    (NodeId)2,
                    new[]
                    {
                        @class((AttributeId)1, "btn", "btn-primary"),
                        href((AttributeId)2, "Add")
                    },
                    text
                    (
                        (NodeId)3,
                        "Add"
                    )
                ),
                h1
                (
                    (NodeId)4,
                    text
                    (
                        (NodeId)5,
                        "All items"
                    )
                ),
                table
                (
                    (NodeId)6,
                    GetInventoryItemNodes(model.InventoryItems)
                )
            );   


    private static Node[] GetInventoryItemNodes(IEnumerable<InventoryItemModel>? inventoryItems) =>
        inventoryItems?
            .Select(inventoryItem =>
                tr
                (
                    (NodeId)7,
                    td
                    (
                        (NodeId)8,
                        navLinkMatchAll
                        (
                            (NodeId)9,      
                            new[]
                            {
                                href((AttributeId)3, $"/Details/{inventoryItem.id}")
                            },
                            text
                            (
                                (NodeId)10,
                                inventoryItem.name ?? string.Empty
                            )
                        )
                    ),
                    // conditional output
                    inventoryItem.activated
                    ?
                        td
                        (
                            (NodeId)11,
                            navLinkMatchAll
                            (
                                (NodeId)12,
                                new[]
                                {
                                    href((AttributeId)4, $"/Deactivate/{inventoryItem.id}")
                                },
                                text
                                (
                                    (NodeId)13,
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
                                (NodeId)15,
                                new[]
                                {
                                    href((AttributeId)5, $"/Activate/{inventoryItem.id}")
                                },
                                text
                                (
                                    (NodeId)16,
                                    "Activate"
                                )
                            )
                        )
                    )
            ).ToArray()
        ??
            Array.Empty<Node>();
}

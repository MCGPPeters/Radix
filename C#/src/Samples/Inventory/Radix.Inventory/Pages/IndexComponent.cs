using Microsoft.AspNetCore.Components;
using Radix.Blazor.Inventory.Interface.Logic;
using Radix.Components;
using Radix.Components.Html;
using Radix.Inventory.Domain;

namespace Radix.Blazor.Inventory.Server.Pages;

[Route("/")]
public class IndexComponent : TaskBasedComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent, Json>
{
    protected override Node View(IndexViewModel currentViewModel) =>
        concat
        (
            navLinkMatchAll
            (
                new[]
                {
                    @class("btn btn-primary"),
                    href("Add")
                },
                text
                (
                    "Add"
                )
            ),
            h1
            (
                text
                (
                    "All items"
                )
            ),
            table
            (
                GetInventoryItemNodes(currentViewModel.InventoryItems)
            )    
        );
    


    private static Node[] GetInventoryItemNodes(IEnumerable<InventoryItemModel>? inventoryItems) =>
        inventoryItems?
            .Select(inventoryItem =>
                tr
                (
                    td
                    (
                        navLinkMatchAll
                        (
                            new[]
                            {
                                href($"/Details/{inventoryItem.id}")
                            },
                            text
                            (
                                inventoryItem.name ?? string.Empty
                            )
                        )
                    ),
                    // conditional output
                    inventoryItem.activated
                    ?
                        td
                        (
                            navLinkMatchAll
                            (
                                new[]
                                {
                                    href($"/Deactivate/{inventoryItem.id}")
                                },
                                text
                                (
                                    "Deactivate"
                                )
                            )
                        )
                    :
                        td
                        (
                            navLinkMatchAll
                            (
                                new[]
                                {
                                    href($"/Activate/{inventoryItem.id}")
                                },
                                text
                                (
                                    "Activate"
                                )
                            )
                        )
                    )
            ).ToArray()
        ??
            Array.Empty<Node>();
}

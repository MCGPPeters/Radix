using Radix.Components;

namespace Radix.Inventory.Pages;

public record DeactivateInventoryItemModel : ViewModel
{

    public string? InventoryItemName { get; set; }
    public string? Reason { get; set; }
}

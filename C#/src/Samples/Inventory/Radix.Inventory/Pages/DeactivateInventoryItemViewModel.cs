using Radix.Components;

namespace Radix.Inventory.Pages;

public record DeactivateInventoryItemViewModel : ViewModel
{

    public string? InventoryItemName { get; set; }
    public string? Reason { get; set; }
}

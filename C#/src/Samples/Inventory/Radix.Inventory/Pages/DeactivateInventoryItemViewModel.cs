using Radix.Components;

namespace Radix.Blazor.Inventory.Server.Pages;

public record DeactivateInventoryItemViewModel : ViewModel
{

    public string? InventoryItemName { get; set; }
    public string? Reason { get; set; }
}

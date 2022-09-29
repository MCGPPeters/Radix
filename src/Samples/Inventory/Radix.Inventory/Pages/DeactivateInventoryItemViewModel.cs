namespace Radix.Inventory.Pages;

public record DeactivateInventoryItemModel
{

    public string? InventoryItemName { get; set; }
    public string? Reason { get; set; }
    public Error[]? Errors { get; internal set; }
}

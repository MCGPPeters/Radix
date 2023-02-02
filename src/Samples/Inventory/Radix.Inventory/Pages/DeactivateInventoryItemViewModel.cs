namespace Radix.Inventory.Pages;

public record DeactivateInventoryItemModel
{

    public string? InventoryItemName { get; set; }
    public string? Reason { get; set; }
    public IEnumerable<Error>? Errors { get; internal set; }
}

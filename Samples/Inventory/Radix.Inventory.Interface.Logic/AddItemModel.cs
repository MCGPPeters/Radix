namespace Radix.Blazor.Inventory.Interface.Logic;

public record AddItemModel
{
    public string InventoryItemName { get; init; } = "";
    public int InventoryItemCount { get; init; }
    public long InventoryItemId { get; init; }
    public IEnumerable<Error> Errors { get; init; } = new List<Error>();
}

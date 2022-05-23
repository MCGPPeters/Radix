using Radix.Components;

namespace Radix.Blazor.Inventory.Interface.Logic;

public record AddInventoryItemModel
{
    public string? InventoryItemName { get; set; }
    public int InventoryItemCount { get; set; }
    public long InventoryItemId { get; set; }
    public IEnumerable<Error> Errors { get; set; }
}

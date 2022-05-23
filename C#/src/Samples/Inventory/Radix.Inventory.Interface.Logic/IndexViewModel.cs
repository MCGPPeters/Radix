using Radix.Components;

namespace Radix.Blazor.Inventory.Interface.Logic;

public record IndexModel : ViewModel
{
    public IndexModel(List<InventoryItemModel> inventoryItems) => InventoryItems = inventoryItems;

    /// <summary>
    ///     This is just an example.. in real life this would be a database or something
    /// </summary>
    public List<InventoryItemModel> InventoryItems { get; set; }
}

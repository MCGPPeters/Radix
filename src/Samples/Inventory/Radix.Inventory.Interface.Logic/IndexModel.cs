namespace Radix.Blazor.Inventory.Interface.Logic;

public record IndexModel
{
    /// <summary>
    ///     This is just an example.. in real life this would be a database or something
    /// </summary>
    public required List<ItemModel> InventoryItems { get; set; }
}

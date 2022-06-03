using Radix.Data;

namespace Radix.Inventory.Domain;

public record DeactivateInventoryItem(string Reason) : InventoryItemCommand
{

    private static Func<string, InventoryItemCommand> New => (reason) =>
        new DeactivateInventoryItem(reason);

    public static Validated<InventoryItemCommand> Create(string? reason) => Valid(New)
        .Apply(!string.IsNullOrEmpty(reason) ? Valid(reason) : Invalid<string>("A reason for deactivation must be provided"));
}

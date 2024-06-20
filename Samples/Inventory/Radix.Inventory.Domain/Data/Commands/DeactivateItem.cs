using Radix.Data;

namespace Radix.Inventory.Domain.Data.Commands;

public record DeactivateItem(string Reason) : InventoryCommand
{

    private static Func<string, InventoryCommand> New => (reason) =>
        new DeactivateItem(reason);

    public static Validated<InventoryCommand> Create(string? reason) => Valid(New)
        .Apply(!string
            .IsNullOrEmpty(reason)
            ? Valid(reason)
            : Invalid<string>("Reason", "A reason for deactivation must be provided"));
}

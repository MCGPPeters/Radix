using Radix.Data;

namespace Radix.Inventory.Domain.Data.Commands;

public record DeactivateItem(string Reason) : ItemCommand
{

    private static Func<string, ItemCommand> New => (reason) =>
        new DeactivateItem(reason);

    public static Validated<ItemCommand> Create(string? reason) => Valid(New)
        .Apply(!string
        .IsNullOrEmpty(reason)
        ? Valid(reason)
        : Invalid<string>("A reason for deactivation must be provided"));
}

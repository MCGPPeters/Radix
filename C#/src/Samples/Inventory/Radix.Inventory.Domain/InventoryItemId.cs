using Radix.Data.Long.Validity;

namespace Radix.Inventory.Domain;

[Validated<long, IsGreaterThanZero>]
public partial record InventoryItemId
{

}

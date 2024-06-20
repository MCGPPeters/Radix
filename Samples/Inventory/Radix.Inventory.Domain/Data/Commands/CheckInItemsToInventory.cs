using Radix.Data;

namespace Radix.Inventory.Domain.Data.Commands;

[ValidatedMember<long, Radix.Data.Number.Validity.IsGreaterThanZero<long>>("Id")]
[ValidatedMember<int, Radix.Data.Number.Validity.IsGreaterThanZero<int>>("Amount")]
public partial record CheckInItemsToInventory : InventoryCommand
{
   
}

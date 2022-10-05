using Radix.Data.Long.Validity;

namespace Radix.Inventory.Domain;

[Validated<long, IsGreaterThanZero<long>>]
public partial record Id
{

}

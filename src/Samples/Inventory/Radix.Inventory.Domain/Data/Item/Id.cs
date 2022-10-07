
using Radix.Data.Number.Validity;

namespace Radix.Inventory.Domain;

[Validated<long, IsGreaterThanZero<long>>]
public partial record Id
{

}

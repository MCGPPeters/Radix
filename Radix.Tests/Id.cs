using Radix.Data.Number.Validity;

namespace Radix.Tests;

[Validated<long, IsGreaterThanZero<long>>]
public partial record Id
{

}

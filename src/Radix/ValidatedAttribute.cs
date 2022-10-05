using Radix.Data;
using Radix.Data.Long.Validity;
using static Radix.Control.Validated.Extensions;

namespace Radix;

public record struct ProductCode { }


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
public class ValidatedAttribute<T, V> : Attribute where V : Validity<T>
{

}

[Validated<long, IsGreaterThanZero<long>>]
public partial record struct ProductQuantity;


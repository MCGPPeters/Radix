using Radix.Data.Number.Validity;

namespace Radix;

public record struct ProductCode { }


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
public class ValidatedAttribute<T, V> : Attribute where V : Validity<T>
{

}

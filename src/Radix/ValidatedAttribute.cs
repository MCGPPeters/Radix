using Radix.Data;

namespace Radix;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
public class ValidatedAttribute<T, V> : Attribute where V : Validity<T>
{

}

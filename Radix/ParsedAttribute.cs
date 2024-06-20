using Radix.Data;

namespace Radix;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public class ParsedAttribute<T, F> : Attribute where F : Read<T> where T : IParsable<T>
{

}



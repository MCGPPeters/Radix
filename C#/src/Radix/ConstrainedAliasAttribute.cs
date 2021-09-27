namespace Radix;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public class ConstrainedAliasAttribute<T, U> : Attribute where U : Validator<T>
{

}

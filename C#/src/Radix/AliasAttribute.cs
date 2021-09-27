namespace Radix;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public class AliasAttribute<T> : Attribute
{

}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
public class ConstrainedAliasAttribute<T, U> : Attribute where U : Validator<T>
{

}

public interface Validator<T>
{
    static abstract Func<T, Validated<T>> Validate { get; }
}

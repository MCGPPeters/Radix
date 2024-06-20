using Radix.Data.Number.Validity;
using Radix.Data.String.Validity;
using Radix.Math.Pure.Numbers;

namespace Radix;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
public class ValidatedAttribute<T, V> : Attribute where V : Validity<T>
{

}

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public class ValidatedMemberAttribute<T, V> : Attribute where V : Validity<T>
{
    public string MemberName { get; }

    public ValidatedMemberAttribute(string memberName)
    {
        MemberName = memberName;
    }
}

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ValidatedPropertyAttribute<T, V> : Attribute where V : Validity<T>
{

}

public class StringLength : ValidatedAttribute<string, LengthIsInClosedInterval>
{
    public StringLength(int lowerBound, int upperBound)
    {
        LengthIsInClosedInterval.UpperBound = (Integer)upperBound;
        LengthIsInClosedInterval.LowerBound = (Integer)lowerBound;
    }
}



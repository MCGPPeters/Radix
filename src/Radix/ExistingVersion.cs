namespace Radix;

public record ExistingVersion(long Value) : Version(Value)
{
    public static implicit operator ExistingVersion(long alias) => new(alias);
}

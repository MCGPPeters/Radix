namespace Radix.Validated;

public sealed record Valid<T>(T Value) : Validated<T>
{
    public static implicit operator Valid<T>(T t) => new(t);
}

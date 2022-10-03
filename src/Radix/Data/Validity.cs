namespace Radix.Data;

public interface Validity<T>
{
    public static abstract Func<string, Func<T, Validated<T>>> Validate { get; }
}

namespace Radix;

public interface Validator<T>
{
    static abstract Func<T, Validated<T>> Validate { get; }
}

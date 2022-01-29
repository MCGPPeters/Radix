namespace Radix.Data;

public interface Validity<T>
{
    public static abstract Validated<T> Validate(T value);
}

namespace Radix.Data;

public interface Validity<T>
{
    public static abstract Validated<T> Validate(T value);

    public static abstract Validated<T> Validate(string name, T value);
}

namespace Radix.Data;

public interface FromString<T>
{
    static abstract Validated<T> Parse(string s);

    static abstract Validated<T> Parse(string s, string validationErrorMessage);
}

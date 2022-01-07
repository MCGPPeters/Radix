namespace Radix.Data;

public interface Read<T>
{
    static abstract Validated<T> Parse(string s);

    static abstract Validated<T> Parse(string s, string validationErrorMessage);
}

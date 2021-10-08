namespace Radix.Data;

public interface FromString<T>
{
    static abstract Result<T, Error> Parse(string s);
}

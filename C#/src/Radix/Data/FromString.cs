namespace Radix.Data;

public interface FromString<T>
{
    static abstract Option<T> Parse(string s, Option<IFormatProvider> provider);

    static abstract bool TryParse(string s, IFormatProvider? provider, out T result);
}

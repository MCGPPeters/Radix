namespace Radix.Data;

public interface FromSpan<T>
{
    static abstract bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out T result);

    static abstract Option<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider);
}

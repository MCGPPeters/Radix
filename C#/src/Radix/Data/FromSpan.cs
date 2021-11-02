namespace Radix.Data;

public interface FromSpan<T>
{
    static abstract Validated<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider);
}

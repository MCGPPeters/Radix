namespace Radix.Data;

public interface ToString<T>
{
    static abstract string Format();

    static abstract string Format(string? format, IFormatProvider? provider);

    static abstract string Format(Option<string> format, Option<IFormatProvider> provider);
}

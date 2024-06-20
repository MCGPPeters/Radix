namespace Radix.Data;

public interface Show<in T>
{
    

    static abstract string Format(T t);

    static abstract string Format(T t, string? format, IFormatProvider? provider);

    static abstract Option<string> Format(T t, Option<string> format, Option<IFormatProvider> provider);
}

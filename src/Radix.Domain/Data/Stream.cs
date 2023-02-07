using Radix.Control.Option;
using Radix.Data;
using Radix.Tests;

namespace Radix.Domain.Data;

public record Stream : Show<Stream>
{
    private const char Separator = '#';

    public required StreamId Id { get; init; }

    public required StreamName Name { get; init; }
    public static string Format(Stream t) => $"{t.Name}{Separator}{t.Id}";

    public static string Format(Stream t, string? format, IFormatProvider? provider) => string.Format(provider, Format(t));

    public static Option<string> Format(Stream t, Option<string> format, Option<IFormatProvider> provider) =>
        from f in format
        from p in provider
        select Format(t, f, p);

    public override string ToString() => Format(this);
}

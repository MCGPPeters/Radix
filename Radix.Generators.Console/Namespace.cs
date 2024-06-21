using Radix.Data.String.Validity;
using Radix.Generators.Attributes;

namespace Radix.Generators.Console;

[Validated<string, ContainsOnlyAlphaNumericsAndHyphens>]
[Validated<string, StartsWithALetter>]
[Validated<string, EndsWithALetterOrNumber>]
[Validated<string, IsNotNullOrEmpty>]
[StringLength(6, 50)]
public readonly partial record struct Namespace { };


[Configuration]
public class PostgresSettings
{
    public required string ConnectionString { get; init; }
}

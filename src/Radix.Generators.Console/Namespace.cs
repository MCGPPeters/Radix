using Radix.Data.String.Validity;

namespace Radix.Generators.Console;

[Validated<string, ContainsOnlyAlphaNumericsAndHyphens>]
[Validated<string, StartsWithALetter>]
[Validated<string, EndsWithALetterOrNumber>]
[Validated<string, IsNotNullOrEmpty>]
[StringLength(6, 50)]
public readonly partial record struct Namespace { };

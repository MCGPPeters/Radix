using Radix.Data.String.Validity;

namespace Radix.Infrastructure.Azure.Data.Names.ServiceBus;

[Validated<string, StartsWithALetter>]
[Validated<string, EndsWithALetterOrNumber>]
[Validated<string, IsNotNullOrEmpty>]
[Validated<string, ContainsOnlyAlphaNumericsAndHyphens>]
public readonly partial record struct Namespace { };

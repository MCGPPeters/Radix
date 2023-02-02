using Radix.Data.String.Validity;

namespace Radix.Infrastructure.Azure.Data.Names;

/// <summary>
/// 
/// </summary>
[Validated<string, ContainsOnlyAlphaNumericsAndHyphens>]
[Validated<string, StartsWithALetter>]
[Validated<string, EndsWithALetterOrNumber>]
[Validated<string, IsNotNullOrEmpty>]
public readonly partial record struct Namespace { };

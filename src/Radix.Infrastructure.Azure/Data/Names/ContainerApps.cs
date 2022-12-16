using Radix.Data.String.Validity;

namespace Radix.Infrastructure.Azure.Data.Names.ContainerApps;

/// <summary>
/// A value must consist of lower case alphanumeric characters, '-' or '.', and must start and end with an alphanumeric character."
/// </summary>
[Validated<string, ContainsOnlyAlphaNumericsHyphensAndDots>]
[Validated<string, EndsWithALetterOrNumber>]
[Validated<string, StartsWithALetterOrNumber>]
[Validated<string, IsNotNullOrEmpty>]
[Validated<string, AllLettersAreLowerCase>]
public readonly partial record struct SecretName { };

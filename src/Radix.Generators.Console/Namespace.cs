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

public class Foo
{
    public int MyProperty { get; set; }

    public string Mama { get; set; }

    public Bar Bar { get; set; }
}

public class Bar
{
    public int MyProperty { get; set; }
}


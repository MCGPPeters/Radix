using Radix.Data.Number.Validity;
using Radix.Data.String.Validity;

namespace Radix.Generators.Console;

[ValidatedMember<string, IsNotNullEmptyOrWhiteSpace>("Id")]
[ValidatedMember<string, IsNotNullEmptyOrWhiteSpace>("Reason")]
[ValidatedMember<int, IsGreaterThanZero<int>>("Count")]
public partial record CreateNewItem
{
    public bool Activated { get; init; }
}

public partial record CreateAnotherItem
{

    [ValidatedProperty<string, IsNotNullEmptyOrWhiteSpace>]
    public string Id { get; init; }

    [ValidatedProperty<string, IsNotNullEmptyOrWhiteSpace>]
    public string Reason { get; init; }

    public bool Activated { get; init; }
}

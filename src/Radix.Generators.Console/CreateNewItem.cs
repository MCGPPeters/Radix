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

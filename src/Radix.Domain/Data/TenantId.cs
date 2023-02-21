namespace Radix.Domain.Data;

[Alias<string>]
public partial record TenantId
{
    public static TenantId Default = (TenantId)"";
}
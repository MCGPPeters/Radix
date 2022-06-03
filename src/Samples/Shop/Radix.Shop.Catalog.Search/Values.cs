using Radix.Data.String.Validity;

namespace Radix.Shop.Catalog.Search
{
    [Validated<string, IsNotNullOrEmpty>]
    public partial record SearchServiceName { }

    [Validated<string, IsNotNullOrEmpty>]
    public partial record SearchIndexName { }

    [Validated<string, IsNotNullOrEmpty>]
    public partial record SearchApiKey { }

    [Validated<string, IsNotNullOrEmpty>]
    public partial record ProductIndexName { }
}

using Azure.Search.Documents.Indexes;

namespace Radix.Shop.Catalog.Search.Index;

public class IndexableProduct
{

    [SearchableField(IsKey = true, IsSortable = true)]
    public string Id { get; set; } 

    [SearchableField(IsSortable = true, IsFilterable = true)]
    public string Title { get; set; }

    [SearchableField()]
    public string Description { get; set; }

    [SimpleField()]
    public string ImageSource { get; set; }

    [SearchableField(IsFilterable = true, IsSortable = true)]
    public string MerchantName { get; set; }

    [SimpleField()]
    public int PriceUnits { get; set; }

    [SimpleField()]
    public int PriceFraction { get; set; }

    [SimpleField(IsSortable = true, IsFacetable = true, IsFilterable = true)]
    public string UnitSize { get; set; }

    [SimpleField(IsSortable = true, IsFacetable = true, IsFilterable = true)]
    public string UnitOfMeasure { get; set; }

    [SimpleField()]
    public string PricePerUnitPriceUnits { get; set; }

    [SimpleField()]
    public string PricePerUnitPriceFraction { get; set; }
}

namespace Radix.Shop.Catalog.Domain;

public record Product
{
    public ProductTitle Title { get; set; }
    public ProductDescription Description { get; set; }
    public ProductImageUri ImageSource { get; set; }
    public MerchantName MerchantName { get; set; }
    public Price Price { get; set; }
    public string UnitSize { get; set; }

    public string UnitOfMeasure { get; set; }
    public Option<Price> PricePerUnit { get; set; }
}

public record Price(PriceUnits Units, PriceFraction Fraction);

public record Merchant(MerchantName Name, MerchantSearchUri SearchUri);



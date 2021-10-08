namespace Radix.Shop.Catalog.Domain;

public record Product(ProductTitle Title, ProductDescription Description, ProductImageUri ImageSource, MerchantName MerchantName, Price Price);

public record Price(PriceUnits Units, PriceFraction Fraction);

public record Merchant(MerchantName Name, MerchantSearchUri SearchUri);



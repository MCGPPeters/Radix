using Radix.Shop.Catalog.Domain;

namespace Radix.Shop.Catalog.Search.Index;

public static class Extensions
{
    public static IndexableProduct ToIndexableProduct(this Product product) =>
        new()
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            ImageSource = product.ImageSource,
            MerchantName = product.MerchantName,
            PriceUnits = product.Price.Units,
            PriceFraction = product.Price.Fraction,
            UnitSize = product.UnitSize.ToString(),
            UnitOfMeasure = product.UnitOfMeasure,
            PricePerUnitPriceUnits = product.PricePerUnit.Units,
            PricePerUnitPriceFraction = product.PricePerUnit.Fraction
        };
}

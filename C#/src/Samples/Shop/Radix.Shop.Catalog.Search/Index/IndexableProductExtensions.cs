using Radix.Shop.Catalog.Domain;

namespace Radix.Shop.Catalog.Search.Index;

public static class IndexableProductExtensions
{
    public static Product ToProduct(this IndexableProduct indexableProduct) =>
        new()
        {
            Title = (ProductTitle)indexableProduct.Title,
            Description = (ProductDescription)indexableProduct.Description,
            ImageSource = (ProductImageUri)indexableProduct.ImageSource,
            MerchantName = (MerchantName)indexableProduct.MerchantName,
            PriceUnits = (PriceUnits)indexableProduct.PriceUnits,
            PriceFraction = (PriceFraction)indexableProduct.PriceFraction,
            UnitSize = indexableProduct.UnitSize,
            UnitOfMeasure = indexableProduct.UnitOfMeasure,
        };

    public static IndexableProduct ToIndexableProduct(this Product product) =>
        new()
        {
            Id = product.Id,
            Title = product.Title,
            Description = product.Description,
            ImageSource = product.ImageSource,
            MerchantName = product.MerchantName,
            PriceUnits = product.PriceUnits,
            PriceFraction = product.PriceFraction,
            UnitSize = product.UnitSize,
            UnitOfMeasure = product.UnitOfMeasure,
        };
}

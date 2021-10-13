using System.Globalization;
using Radix.Nullable;
using static Radix.Nullable.Extensions;
using static Radix.Option.Extensions;

namespace Radix.Shop.Catalog.Domain;

public class ProductDTO
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string ImageSource { get; set; }
    public string MerchantName { get; set; }
    public string PriceUnits { get; set; }
    public string PriceFraction { get; set; }
    public string UnitSize { get; set; }
    public string UnitOfMeasure { get; set; }
}
        
public static class Extensions
{
    public static Product ToProduct(this ProductDTO productDTO)
    {
        //var unitPrice = from unitSize in productDTO.UnitSize.AsOption()
        //                from priceUnits in productDTO.PriceUnits.AsOption()
        //                from priceFraction in productDTO.PriceFraction.AsOption()
        //                let pricePerUnit = decimal.Parse($"{priceUnits},{priceFraction}") / decimal.Parse(unitSize)
        //                let ppu = pricePerUnit.ToString().Split(',')
        //                let units = ppu[0]
        //                let fraction = ppu[1]
        //                select new Price((PriceUnits)units, (PriceFraction)fraction);

        return new Product
        {
            Title = (ProductTitle)productDTO.Title,
            Description = (ProductDescription)productDTO.Description,
            ImageSource = (ProductImageUri)productDTO.ImageSource,
            MerchantName = (MerchantName)productDTO.MerchantName,
            UnitOfMeasure = productDTO.UnitOfMeasure,
            UnitSize = productDTO.UnitSize,
            Price = new Price((PriceUnits)productDTO.PriceUnits, (PriceFraction)productDTO.PriceFraction),
            // PricePerUnit = unitPrice
        };
    }
}


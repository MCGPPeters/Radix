namespace Radix.Shop.Catalog.Domain
{
    public class ProductDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }
        public string MerchantName { get; set; }
        public string PriceUnits { get; set; }
        public string PriceFraction { get; set; }
    }
        
    public static class Extensions
    {
        public static Product ToProduct(this ProductDTO productDTO)
        {
            return new Product((ProductTitle)productDTO.Title, (ProductDescription)productDTO.Description, (ProductImageUri)productDTO.ImageSource, (MerchantName)productDTO.MerchantName, new Price((PriceUnits)int.Parse(productDTO.PriceUnits), (PriceFraction) int.Parse(productDTO.PriceFraction)));
        }
    }
}

namespace Radix.Shop.Sales
{
    public record struct ProductType(string Value) : Alias<ProductType, string>
    {
        public static implicit operator string(ProductType type) => type.Value;
        public static implicit operator ProductType(string type) => new(type);
    }
}

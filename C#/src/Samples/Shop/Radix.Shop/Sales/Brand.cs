namespace Radix.Shop.Sales
{
    public record Brand(string Value) : Alias<Brand, string>
    {
        public static implicit operator string(Brand brand) => brand.Value;
        public static implicit operator Brand(string brand) => new(brand);
    }
}

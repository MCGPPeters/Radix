namespace Radix.Shop.Pages
{
    public record Brand : Alias<string>
    {
        public Brand(string Value) : base(Value)
        {
        }

        public static implicit operator string(Brand brand) => brand.Value;
        public static implicit operator Brand(string brand) => new(brand);
        
    }
}

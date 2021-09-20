namespace Radix.Shop.Sales
{
    public record Product
    {
        public Id Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}

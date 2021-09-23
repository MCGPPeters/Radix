using Radix.Components;
using Radix.Shop.Sales;

namespace Radix.Shop.Pages
{
    public record IndexViewModel : ViewModel
    {
        public IndexViewModel(Func<IEnumerable<Brand>, IEnumerable<ProductType>, IAsyncEnumerable<Product>> getFilteredProducts)
        {
            Types = new List<ProductType>()
            {
                (ProductType)"Mug",
                (ProductType)"Sheet",
                (ProductType)"T-Shirt"
            };

            Brands = new List<Brand>()
            {
                (Brand)".NET",
                (Brand)"Azure",
                (Brand)"Other"
            };
            GetFilteredProducts = getFilteredProducts;
        }
        public string TypeFilter { get; internal set; }
        public string BrandFilter { get; internal set; }
        public List<Product> FilteredProducts { get; internal set; } = new List<Product>();
        public IEnumerable<ProductType> Types { get; internal set; }
        public IEnumerable<Brand> Brands { get; internal set; }

        public Func<IEnumerable<Brand>, IEnumerable<ProductType>, IAsyncEnumerable<Product>> GetFilteredProducts { get; }

        internal void AddToBasket(Product product) => throw new NotImplementedException();
    }
}

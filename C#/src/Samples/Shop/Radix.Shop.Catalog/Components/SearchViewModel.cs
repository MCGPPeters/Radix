using Radix.Components;

namespace Radix.Shop.Catalog.Components
{
    public record SearchViewModel : ViewModel
    {
        public List<Product> Products { get; internal set; }
        public string SearchTerm { get; internal set; }

        public Search<Product> Search { get; internal set; }
    }
}

using Radix.Components;
using Radix.Shop.Catalog.Domain;

namespace Radix.Shop.Catalog.Components;

public record SearchViewModel : ViewModel
{
    public List<Product> Products { get; internal set; } = new List<Product>();
    public string SearchTerm { get; internal set; }

    public Search<Product> Search { get; internal set; }
}

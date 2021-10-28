using System.Threading.Channels;
using Radix.Components;
using Radix.Shop.Catalog.Domain;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

public record SearchViewModel : ViewModel
{
    public SearchViewModel(Channel<SearchTerm> crawlingMessageChannel)
    {
        CrawlingMessageChannel = crawlingMessageChannel;
    }
    public List<Product> Products { get; internal set; } = new List<Product>();
    public string SearchTerm { get; internal set; }

    public Search<Product> Search { get; set; }
    public Channel<SearchTerm> CrawlingMessageChannel { get; }

    public async Task ExecuteSearch()
    {
        Products = new List<Product>();

        await CrawlingMessageChannel.Writer.WriteAsync((SearchTerm)SearchTerm);

        await foreach (var product in Search((SearchTerm)SearchTerm))
        {
            Products.Add(product);
        };
    }
}

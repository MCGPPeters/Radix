using System.Threading.Channels;
using Radix.Components;
using Radix.Shop.Catalog.Domain;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

public record SearchModel
{
    public SearchModel(Channel<SearchTerm> crawlingMessageChannel)
    {
        CrawlingMessageChannel = crawlingMessageChannel;
    }
    public List<ProductModel> Products { get; internal set; }

    public string SearchTerm { get; internal set; }

    public Search<ProductModel> Search { get; set; }
    public Channel<SearchTerm> CrawlingMessageChannel { get; }

    public async IAsyncEnumerable<ProductModel> ExecuteSearch(SearchTerm searchTerm)
    {
        await CrawlingMessageChannel.Writer.WriteAsync(searchTerm);

        await foreach (var product in Search((SearchTerm)SearchTerm))
        {
            yield return product;
        }
    }    
}

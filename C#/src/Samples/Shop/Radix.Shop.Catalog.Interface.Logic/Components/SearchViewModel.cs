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
    public List<ProductViewModel> Products { get; internal set; } = new List<ProductViewModel>();
    public string SearchTerm { get; internal set; }

    public Search<ProductViewModel> Search { get; set; }
    public Channel<SearchTerm> CrawlingMessageChannel { get; }

    public async Task ExecuteSearch()
    {
        Products = new List<ProductViewModel>();

        await CrawlingMessageChannel.Writer.WriteAsync((SearchTerm)SearchTerm);

        await foreach (var product in Search((SearchTerm)SearchTerm))
        {
            Products.Add(product);
        };
    }

    public Radix.Components.Nodes.Component GetMerchentLogo(string merchantName) =>
        merchantName switch
        {
            "Albert Heijn" =>
                component<AH.LogoReference>(Enumerable.Empty<Radix.Components.Attribute>()),
            "Jumbo" =>
                component<Jumbo.LogoReference>(Enumerable.Empty<Radix.Components.Attribute>()),
            _ => throw new NotImplementedException()

        };
    
}

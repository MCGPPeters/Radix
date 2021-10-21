using Azure;
using Azure.Search.Documents.Indexes;

namespace Radix.Shop.Catalog.Search.Index;

public record ClientSettings(SearchServiceName ServiceName, SearchIndexName IndexName, SearchApiKey ApiKey);

public class Client
{
    private static SearchIndexClient s_searchIndexClient;

    private static Client s_client;
    private Client(ClientSettings clientSettings)
    {
        // Create a SearchIndexClient to send create/delete index commands
        var serviceEndpoint = new Uri($"https://{clientSettings.ServiceName}.search.windows.net/");
        var credential = new AzureKeyCredential(clientSettings.ApiKey);
        SearchIndexClient = new SearchIndexClient(serviceEndpoint, credential);
    }

    public SearchIndexClient SearchIndexClient { get => s_searchIndexClient; private set => s_searchIndexClient = value; }

    public static Client Create(ClientSettings clientSettings)
    {
        if (s_client is null)
        {
            s_client = new Client(clientSettings);
        }
        return s_client;
    }
}

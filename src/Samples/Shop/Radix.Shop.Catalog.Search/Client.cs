using Azure;
using Azure.Search.Documents;

namespace Radix.Shop.Catalog.Search
{


    public class Client
    {
        private static SearchClient? s_searchClient;
        private static Client? s_client;
        private Client(ClientSettings clientSettings)
        {
            // Create a SearchIndexClient to send create/delete index commands
            Uri serviceEndpoint = new Uri($"https://{clientSettings.ServiceName}.search.windows.net/");
            AzureKeyCredential credential = new AzureKeyCredential(clientSettings.ApiKey);

            // Create a SearchClient to load and query documents
            s_searchClient = new SearchClient(serviceEndpoint, clientSettings.SearchIndexName, credential);
        }

        public SearchClient SearchClient { get => s_searchClient; private set => s_searchClient = value; }

        public static Client Create(ClientSettings clientSettings)
        {
            if (s_client is null)
            {
                s_client = new Client(clientSettings);
            }
            return s_client;
        }       
    }
}

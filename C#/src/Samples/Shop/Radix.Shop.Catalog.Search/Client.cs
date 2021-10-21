using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;

namespace Radix.Shop.Catalog.Search
{
    public record ClientSettings(SearchServiceName ServiceName, SearchApiKey ApiKey, SearchIndexName SearchIndexName);

    public class Client
    {
        private static SearchClient s_searchClient;
        private static Client s_client;
        private Client(ClientSettings clientSettings)
        {
            // Create a SearchIndexClient to send create/delete index commands
            Uri serviceEndpoint = new Uri($"https://{clientSettings.ServiceName}.search.windows.net/");
            AzureKeyCredential credential = new AzureKeyCredential(clientSettings.ApiKey);
            SearchIndexClient adminClient = new SearchIndexClient(serviceEndpoint, credential);

            // Create a SearchClient to load and query documents
            SearchClient = new SearchClient(serviceEndpoint, clientSettings.SearchIndexName, credential);
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

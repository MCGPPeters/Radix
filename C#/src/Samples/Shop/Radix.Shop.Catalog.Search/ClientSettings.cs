using Radix.Validated;
using static Radix.Validated.Extensions;

namespace Radix.Shop.Catalog.Search
{
    public record ClientSettings
    {
        private ClientSettings(string ServiceName, string ApiKey, string SearchIndexName)
        {
            this.ServiceName = ServiceName;
            this.ApiKey = ApiKey;
            this.SearchIndexName = SearchIndexName;
        }

        private static Func<string, string, string, ClientSettings> New =>
            (serviceName, apiKey, searchIndexName) =>
                new ClientSettings(serviceName, apiKey, searchIndexName);

           
        public static Validated<ClientSettings> Create(string? serviceName, string? apiKey, string? searchIndexName) =>
            Valid(New)
            .Apply(serviceName is not null ? Valid(serviceName) : Invalid<string>("The service name key may not be null"))
            .Apply(apiKey is not null ? Valid(apiKey) : Invalid<string>("The api key may not be null"))
            .Apply(searchIndexName is not null ? Valid(searchIndexName) : Invalid<string>("The name of the search index can not be empty"));

        public string ServiceName { get; }
        public string ApiKey { get; }
        public string SearchIndexName { get; }
    }
}

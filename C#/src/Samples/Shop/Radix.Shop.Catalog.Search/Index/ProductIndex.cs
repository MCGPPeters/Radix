using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace Radix.Shop.Catalog.Search.Index
{
    public static class ProductIndex
    {
        public static async Task Create(SearchIndexClient indexClient, SearchIndexName searchIndexName)
        {
            var fieldBuilder = new FieldBuilder();
            IList<SearchField>? searchFields = fieldBuilder.Build(typeof(IndexableProduct));

            var definition = new SearchIndex(searchIndexName, searchFields);

            var suggester = new SearchSuggester("sg", new[] { nameof(IndexableProduct.Title), nameof(IndexableProduct.Description) });
            definition.Suggesters.Add(suggester);

            var _ = await indexClient.CreateOrUpdateIndexAsync(definition);
        }
    }
}

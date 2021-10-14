using System.Text.Json;
using System.Threading.Channels;

namespace Radix.Shop.Catalog.Domain;

public delegate IAsyncEnumerable<T> Search<T>(SearchTerm searchTerm);

public delegate Search<Product> SearchMerchant(Merchant merchant);

public static class Workflows
{
    public static Search<Product> SearchAll(params Search<Product>[] searches) =>
        Search.All(searches); 
}

public static class Search
{
    public static Search<T> All<T>(params Search<T>[] searches) =>
            searchTerm =>
                SearchAllInternal(searchTerm, searches);

    private static async IAsyncEnumerable<T> SearchAllInternal<T>(SearchTerm searchTerm, Search<T>[] searches)
    {
        var channel = Channel.CreateUnbounded<T>();

        Parallel.ForEach(searches, async search =>
        {
            await foreach (var result in search(searchTerm))
                await channel.Writer.WriteAsync(result);
        });

        await foreach (var result in channel.Reader.ReadAllAsync())
            yield return result;
    }
}

public static class AH
{
    public static Func<HttpClient, Func<Uri, Search<Product>>> ConfigureSetHttpClient =>
        httpClient =>
            uri =>
                searchTerm =>
                    SearchProducts(uri, httpClient, searchTerm);

    private static async IAsyncEnumerable<Product> SearchProducts(Uri uri, HttpClient httpClient, SearchTerm searchTerm)
    {
        var searchQuery = $"{uri}{searchTerm}";
        var response = await httpClient.GetStreamAsync(searchQuery).ConfigureAwait(false);
        IAsyncEnumerable<IndexProduct?> products = JsonSerializer.DeserializeAsyncEnumerable<IndexProduct>(response, new JsonSerializerOptions { DefaultBufferSize = 128, PropertyNameCaseInsensitive = true });
        await foreach (var productDTO in products)
        {
            if (productDTO is not null) yield return productDTO.ToProduct();
        }
    }
}

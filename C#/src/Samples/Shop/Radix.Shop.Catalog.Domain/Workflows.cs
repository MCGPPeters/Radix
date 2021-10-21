using System.Threading.Channels;

namespace Radix.Shop.Catalog.Domain;

public delegate IAsyncEnumerable<T> Search<T>(SearchTerm searchTerm);

public delegate Task<Unit> Crawl(SearchTerm searchTerm);

public static class Workflows
{
    public static Crawl CrawlAll(params Crawl[] crawls) =>
        searchTerm =>
        {
            Parallel.ForEach(crawls, async crawl => await crawl(searchTerm));
            return Task.FromResult(Unit.Instance);
        };

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

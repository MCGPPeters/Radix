using Azure.Storage.Queues;
using Radix.Shop.Catalog.Domain;

namespace Radix.Shop;

public static class AH
{
    public static Func<QueueClient, Crawl> ConfigureCrawl =>
        queueClient =>
                searchTerm =>
                    CrawlProducts(queueClient, searchTerm);

    private static async Task<Unit> CrawlProducts(QueueClient queueClient, SearchTerm searchTerm)
    {
        if (queueClient.Exists())
        {
            await queueClient.SendMessageAsync(searchTerm);
        }
        return Unit.Instance;
    }
}

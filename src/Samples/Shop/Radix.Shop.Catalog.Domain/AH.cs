using Azure.Storage.Queues;
using Radix.Data;

namespace Radix.Shop.Catalog.Domain;

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

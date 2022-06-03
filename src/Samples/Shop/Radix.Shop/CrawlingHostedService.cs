using System;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Radix.Shop.Catalog.Domain;

namespace Radix.Shop;

public class CrawlingHostedService : BackgroundService
{
    public CrawlingHostedService(IConfiguration configuration, Channel<SearchTerm> crawlingMessageChannel, Crawl crawl)
    {
        Configuration = configuration;
        SearchTermChannel = crawlingMessageChannel;
        Crawl = crawl;
    }

    public IConfiguration Configuration { get; }
    public Channel<SearchTerm> SearchTermChannel { get; }
    public Crawl Crawl { get; }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var searchTerm in SearchTermChannel.Reader.ReadAllAsync(stoppingToken))
        {
            await Workflows.CrawlAll(Crawl)(searchTerm);
        }
    }
}

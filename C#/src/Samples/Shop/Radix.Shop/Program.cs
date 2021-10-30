using Radix.Shop.Catalog.Interface.Logic.Components;
using Radix.Shop.Pages;
using Radix.Shop.Shared;
using Radix.Shop.Catalog.Domain;
using Radix.Shop.Catalog.Search;
using Radix.Shop.Catalog.Search.Index;
using Azure.Search.Documents.Models;
using System.Threading.Channels;
using Radix.Shop;
using Azure.Storage.Queues;
using Radix.Validated;
using System.Configuration;
using Azure.Search.Documents.Indexes;
using Radix.Data.String;
using Radix.Collections.Generic.Enumerable;
using Radix;
using static Radix.Async.Extensions;
using Azure.Search.Documents;

var builder = WebApplication.CreateBuilder(args);

// Ensure base64 encoding is set
QueueClient ahQueueClient = new QueueClient(builder.Configuration["AH_CONNECTION_STRING"], builder.Configuration["AH_QUEUE_NAME"], new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64});
// Create the queue if it doesn't already exist
ahQueueClient.CreateIfNotExists();

Crawl crawlAH = AH.ConfigureCrawl(ahQueueClient);

var result =
        from indexName in SearchIndexName.Create(builder.Configuration[Constants.SearchIndexName])
        from searchServiceName in SearchServiceName.Create(builder.Configuration[Constants.SearchServiceName])
        from searchApiKey in SearchApiKey.Create(builder.Configuration[Constants.SearchApiKey])
        let serviceEndpoint = new Uri($"https://{searchServiceName}.search.windows.net/")
        let credentials = new Azure.AzureKeyCredential(searchApiKey)
        let indexClient = new SearchIndexClient(serviceEndpoint, credentials)
        select (ProductIndex.Create(indexClient, indexName), new SearchClient(serviceEndpoint, indexName, credentials));

switch (result)
{
    case Valid<(Task, SearchClient)> (var (createIndex, searchClient)) :
        await createIndex.Return().Retry(Backoff.Exponentially());
        async IAsyncEnumerable<Product> SearchProducts(SearchTerm searchTerm)
        {
            Azure.Response<SearchResults<IndexableProduct>> response = await searchClient.SearchAsync<IndexableProduct>(searchTerm);
            await foreach (var result in response.Value.GetResultsAsync())
            {
                yield return result.Document.ToProduct();
            }
        };
        var channel = Channel.CreateUnbounded<SearchTerm>();
        var searchViewModel = new SearchViewModel(channel) { Search = SearchProducts };

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<NavMenuViewModel>();
        builder.Services.AddSingleton<IndexViewModel>();
        builder.Services.AddSingleton(searchViewModel);
        builder.Services.AddSingleton(Workflows.CrawlAll(crawlAH));
        builder.Services.AddSingleton(channel);

        builder.Services.AddHostedService<CrawlingHostedService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();


        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();

        break;
    case Invalid<(Task, SearchClient)> (var errors) :
        throw new ConfigurationErrorsException(errors.Aggregate<string, Concat>());
    default:
        break;
}






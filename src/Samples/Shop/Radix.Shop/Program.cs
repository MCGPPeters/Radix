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
using Radix.Data;
using System.Configuration;
using Azure.Search.Documents.Indexes;
using Radix.Data.String;
using Radix.Data.Collections.Generic.Enumerable;
using Radix;
using static Radix.Control.Task.Extensions;
using static Radix.Control.Validated.Extensions;
using Azure.Search.Documents;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Radix.Control.Task;
using Radix.Shop.Catalog.Interface.Logic.Components.Jumbo;
using Radix.Interaction.Data;

var builder = WebApplication.CreateBuilder(args);

//Retrieve the Connection String from the secrets manager 
var connectionString = builder.Configuration.GetConnectionString("AZURE_APP_CONFIG_CONNECTION_STRING");

builder.Configuration.AddAzureAppConfiguration(options =>
    options
        .Connect(connectionString)
        .ConfigureRefresh(configure =>
            configure
                .Register("STORAGE_ACCOUNT_PRIMARY_CONNECTION_STRING")
                .Register("AH_QUEUE_NAME")
                .Register("JUMBO_QUEUE_NAME")
                .SetCacheExpiration(TimeSpan.FromDays(1))
            )
        );

// Ensure base64 encoding is set
QueueClient ahQueueClient = new QueueClient(builder.Configuration["STORAGE_ACCOUNT_PRIMARY_CONNECTION_STRING"], builder.Configuration["AH_QUEUE_NAME"], new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64});
QueueClient jumboQueueClient = new QueueClient(builder.Configuration["STORAGE_ACCOUNT_PRIMARY_CONNECTION_STRING"], builder.Configuration["JUMBO_QUEUE_NAME"], new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64});
// Create the queue if it doesn't already exist
ahQueueClient.CreateIfNotExists();
jumboQueueClient.CreateIfNotExists();

Crawl crawlAH = AH.ConfigureCrawl(ahQueueClient);
Crawl crawlJumbo = Jumbo.ConfigureCrawl(jumboQueueClient);

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
        async IAsyncEnumerable<ProductModel> SearchProducts(SearchTerm searchTerm)
        {
            Azure.Response<SearchResults<IndexableProduct>> response = await searchClient.SearchAsync<IndexableProduct>(searchTerm);
            await foreach (var result in response.Value.GetResultsAsync())
            {
                var productModel = new ProductModel
                {
                    Id = result.Document.Id,
                    Title = result.Document.Title,
                    Description = result.Document.Description,
                    ImageSource = result.Document.ImageSource,
                    MerchantName = result.Document.MerchantName,
                    PriceFraction = result.Document.PriceFraction.ToString(),
                    PriceUnits = result.Document.PriceUnits.ToString(),
                    PricePerUnitPriceFraction = result.Document.PricePerUnitPriceFraction,
                    PricePerUnitPriceUnits = result.Document.PricePerUnitPriceUnits,
                    UnitOfMeasure = result.Document.UnitOfMeasure,
                    UnitSize = result.Document.UnitSize,
                };
                yield return productModel;
            }
        };
        var channel = Channel.CreateUnbounded<SearchTerm>();
        var searchModel = new SearchModel(channel) { Search = SearchProducts };

        // Add services to the container.
        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<IndexModel>();
        builder.Services.AddSingleton<LogoReferenceModel>();
        builder.Services.AddSingleton(searchModel);
        builder.Services.AddSingleton(Workflows.CrawlAll(crawlAH, crawlJumbo));
        builder.Services.AddSingleton(channel);

        builder.Services.AddHostedService<CrawlingHostedService>();

        var openTelemetry = Sdk.CreateTracerProviderBuilder()
                .AddSource("Radix.Shop")
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddJaegerExporter()
                .Build();
        builder.Services.AddSingleton(openTelemetry);
        builder.Services.AddSingleton<IndexModel>();
        builder.Services.AddSingleton<ListModel>();
        builder.Services.AddSingleton(new CarouselModel("frequentItems", new CarouselOptions(), text((NodeId)1, "foo"), text((NodeId)2, "bar")));

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






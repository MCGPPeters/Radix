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

var builder = WebApplication.CreateBuilder(args);

// Ensure base64 encoding is set
QueueClient ahQueueClient = new QueueClient(builder.Configuration["AH_CONNECTION_STRING"], builder.Configuration["AH_QUEUE_NAME"], new QueueClientOptions { MessageEncoding = QueueMessageEncoding.Base64});
// Create the queue if it doesn't already exist
ahQueueClient.CreateIfNotExists();

Crawl crawlAH = AH.ConfigureCrawl(ahQueueClient);

var indexName = (SearchIndexName)builder.Configuration[Constants.SearchIndexName];
var indexClient = Radix.Shop.Catalog.Search.Index.Client.Create(new Radix.Shop.Catalog.Search.Index.ClientSettings((SearchServiceName)builder.Configuration[Constants.SearchServiceName], indexName, (SearchApiKey)builder.Configuration[Constants.SearchApiKey]));
await ProductIndex.Create(indexClient.SearchIndexClient, indexName);

var searchClient = Radix.Shop.Catalog.Search.Client.Create(new Radix.Shop.Catalog.Search.ClientSettings((SearchServiceName)builder.Configuration[Constants.SearchServiceName], (SearchApiKey)builder.Configuration[Constants.SearchApiKey], (SearchIndexName)builder.Configuration[Constants.SearchIndexName]));

async IAsyncEnumerable<Product> SearchProducts(SearchTerm searchTerm) 
{
    Azure.Response<SearchResults<IndexableProduct>> response = await searchClient.SearchClient.SearchAsync<IndexableProduct>(searchTerm);
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

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using Azure.Search.Documents;
using Radix.Validated;
using Radix.Shop.Catalog.Search;
using Radix.Collections.Generic.Enumerable;
using Radix.Data.String;
using Microsoft.Playwright;
using OpenTelemetry;
using OpenTelemetry.Trace;
using Microsoft.Extensions.Configuration;
using System.Reflection;

[assembly: FunctionsStartup(typeof(Radix.Shop.Catalog.Crawling.AH.Startup))]


namespace Radix.Shop.Catalog.Crawling.AH;
public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;

        var result =
            from indexName in SearchIndexName.Create(configuration[Constants.SearchIndexName])
            from searchServiceName in SearchServiceName.Create(configuration[Constants.SearchServiceName])
            from searchApiKey in SearchApiKey.Create(configuration[Constants.SearchApiKey])
            let serviceEndpoint = new Uri($"https://{searchServiceName}.search.windows.net/")
            let credentials = new Azure.AzureKeyCredential(searchApiKey)
            select new SearchClient(serviceEndpoint, indexName, credentials);

        switch (result)
        {
            case Valid<SearchClient>(var searchClient):
                builder.Services.AddSingleton(searchClient);
                break;
            case Invalid<SearchClient>(var errors):
                throw new Exception(errors.Aggregate<string, Concat>());
            default:
                break;
        }

        builder.Services.AddSingleton(serviceProvider =>
        {
            IPlaywright? playwright = Playwright.CreateAsync().GetAwaiter().GetResult();
            return playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Timeout = 0 }).GetAwaiter().GetResult();
        });

        builder.Services.AddSingleton(serviceProvider =>
        {
            var browser = serviceProvider.GetService<IBrowser>();
            return browser.NewContextAsync(new BrowserNewContextOptions { IgnoreHTTPSErrors = true }).GetAwaiter().GetResult();
        });

        var openTelemetry = Sdk.CreateTracerProviderBuilder()
                .AddSource("Radix.Shop.Catalog.Crawling.AH")
                .AddAspNetCoreInstrumentation()
                // .AddHttpClientInstrumentation()
                // .SetSampler(new AlwaysOnSampler())
                .AddConsoleExporter()
                .Build();
        builder.Services.AddSingleton(openTelemetry);

    }
}

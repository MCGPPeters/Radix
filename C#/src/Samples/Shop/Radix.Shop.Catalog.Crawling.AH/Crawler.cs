using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Radix.Shop.Catalog.Domain;
using Microsoft.Playwright;
using System.Collections.Generic;
using Azure.Search.Documents.Models;
using System.Linq;
using Radix.Shop.Catalog.Search.Index;
using Microsoft.Extensions.Configuration;
using Radix.Shop.Catalog.Search;
using System;
using System.IO;
using System.Data.HashFunction.FNV;

namespace Radix.Shop.Catalog.Crawling.AH;


public class Crawler
{

    public static IBrowser Browser { get; set; }
    public static IBrowserContext Context { get; private set; }

    private static Search.Client SearchClient;
    private static IFNV1a s_fnv;

    [FunctionName("crawl")]
    public async Task Run([QueueTrigger("stq-radix-samples-shop-catalog-ah-searchterms", Connection = "AH_CONNECTION_STRING")]string searchTerm, ILogger log)
    {
        var searchServiceName = (SearchServiceName)Environment.GetEnvironmentVariable(Constants.SearchServiceName);
        var searchApiKey = (SearchApiKey)Environment.GetEnvironmentVariable(Constants.SearchApiKey);
        var searchIndexName = (SearchIndexName)Environment.GetEnvironmentVariable(Constants.SearchIndexName);

        if (SearchClient is null) SearchClient = Search.Client.Create(new Search.ClientSettings(searchServiceName, searchApiKey,searchIndexName));
        if (Browser is null)
        {
            IPlaywright? playwright = await Playwright.CreateAsync();
            
            Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions {Timeout = 0});
        }

        if (Context is null) Context =  await Browser.NewContextAsync(new BrowserNewContextOptions { IgnoreHTTPSErrors = true });

        const string merchant = "Albert Heijn";

        int numberOfPagesToCrawl = int.Parse(Environment.GetEnvironmentVariable(Constants.NumberOfPagesToCrawl));
        var page = await Context.NewPageAsync();
        await page.GotoAsync($"https://www.ah.nl/zoeken?query={System.Web.HttpUtility.UrlEncode(searchTerm)}&page={numberOfPagesToCrawl}", new PageGotoOptions { Timeout = 0 });
        var productElements = await page.QuerySelectorAllAsync("//*[@data-testhook='product-card']");

        var products = new List<Product>();

        foreach (var productElement in productElements)
        {
            using Task<IElementHandle> getProductTitle = productElement.QuerySelectorAsync("css=[data-testhook='product-title']");
            using Task<IElementHandle> getPriceUnits = productElement.QuerySelectorAsync("css=[class='price-amount_integer__1cJgL']");
            using Task<IElementHandle> getPriceFraction = productElement.QuerySelectorAsync("css=[class='price-amount_fractional__2wVIK']");
            using Task<IElementHandle> getUnitSize = productElement.QuerySelectorAsync("css=[data-testhook='product-unit-size']");
            using Task<IElementHandle> getImageSource = productElement.QuerySelectorAsync("css=[data-testhook='product-image']");

            await Task.WhenAll(getImageSource, getPriceFraction, getUnitSize, getPriceUnits, getProductTitle);

            var productTitle = await getProductTitle.ConfigureAwait(false);
            var productPriceUnits = await getPriceUnits.ConfigureAwait(false);
            var productPriceFraction = await getPriceFraction.ConfigureAwait(false);
            var unitSize = await getUnitSize.ConfigureAwait(false);
            var imageElement = await getImageSource.ConfigureAwait(false);
            var imageSource = await imageElement.GetAttributeAsync("src").ConfigureAwait(false);

            string scrapedPriceUnits = await productPriceUnits.TextContentAsync().ConfigureAwait(false) ?? "";
            string scrapedPriceFraction = await productPriceFraction.TextContentAsync().ConfigureAwait(false) ?? "";
            string scrapedProductTitle = await productTitle.TextContentAsync().ConfigureAwait(false) ?? "";
            string scrapedUnitSize = "";
            string scrapedUnitOfMeasure = "";

            if (unitSize is not null)
            {
                var scrapedUnitSizeParts = (await unitSize.TextContentAsync().ConfigureAwait(false))?.Split(" ");
                scrapedUnitSize = scrapedUnitSizeParts[0] is not null ? scrapedUnitSizeParts[0] : "";
                scrapedUnitOfMeasure = scrapedUnitSizeParts[1] is not null ? scrapedUnitSizeParts[1] : "";
            }

            // make sure to create a hash per merchant
            byte[] titleBytes = System.Text.Encoding.UTF8.GetBytes(scrapedProductTitle + merchant);
            using var stream = new MemoryStream(titleBytes);
            if (s_fnv is null) s_fnv = FNV1aFactory.Instance.Create(FNVConfig.GetPredefinedConfig(32));
            var hash = await s_fnv.ComputeHashAsync(stream);

            var product =
                new Product
                {
                    Id = hash.AsHexString(),
                    Title = (ProductTitle)scrapedProductTitle,
                    Description = (ProductDescription)"",
                    ImageSource = (ProductImageUri)imageSource,
                    MerchantName = (MerchantName)merchant,
                    PriceUnits = (PriceUnits)int.Parse(scrapedPriceUnits),
                    PriceFraction = (PriceFraction)int.Parse(scrapedPriceFraction),
                    UnitSize = scrapedUnitSize,
                    UnitOfMeasure = scrapedUnitOfMeasure
                };

            products.Add(product);
        }

      IndexDocumentsBatch<IndexableProduct> batch =
            IndexDocumentsBatch.Create(products.Select(product => IndexDocumentsAction.Upload(product.ToIndexableProduct())).ToArray());
        IndexDocumentsResult result = SearchClient.SearchClient.IndexDocuments(batch);

        await page.CloseAsync();          
    }
}

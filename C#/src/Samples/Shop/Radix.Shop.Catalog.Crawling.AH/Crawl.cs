using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Radix.Shop.Catalog.Domain;
using Microsoft.Playwright;
using Azure.Search.Documents.Models;
using Radix.Shop.Catalog.Search.Index;
using Radix.Shop.Catalog.Search;
using System;
using Radix.Validated;
using Azure.Search.Documents;
using Radix.Collections.Generic.Enumerable;
using Radix.Data.String;

namespace Radix.Shop.Catalog.Crawling.AH;


public class Crawl
{

    public static IBrowser? Browser { get; set; }
    public static IBrowserContext? Context { get; private set; }

    public static SearchClient SearchClient { get; set; }

    [FunctionName("crawl")]
    public static async Task Run([QueueTrigger("stq-radix-samples-shop-catalog-ah-searchterms", Connection = "AH_CONNECTION_STRING")]string searchTerm, ILogger log)
    {
        if (SearchClient is null)
        {
            var result =
                from indexName in SearchIndexName.Create(Environment.GetEnvironmentVariable(Constants.SearchIndexName))
                from searchServiceName in SearchServiceName.Create(Environment.GetEnvironmentVariable(Constants.SearchServiceName))
                from searchApiKey in SearchApiKey.Create(Environment.GetEnvironmentVariable(Constants.SearchApiKey))
                let serviceEndpoint = new Uri($"https://{searchServiceName}.search.windows.net/")
                let credentials = new Azure.AzureKeyCredential(searchApiKey)
                select new SearchClient(serviceEndpoint, indexName, credentials);

            switch (result)
            {
                case Valid<SearchClient>(var searchClient):
                    SearchClient = searchClient;
                    break;
                case Invalid<SearchClient>(var errors):
                    throw new Exception(errors.Aggregate<string, Concat>());
                default:
                    break;
            }
        }

        if (Browser is null)
        {
            IPlaywright? playwright = await Playwright.CreateAsync();

            Browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Timeout = 0 }).ConfigureAwait(false);
        }

        if (Context is null) Context = await Browser.NewContextAsync(new BrowserNewContextOptions { IgnoreHTTPSErrors = true }).ConfigureAwait(false);

        const string merchantName = "Albert Heijn";

        int numberOfPagesToCrawl = int.Parse(Environment.GetEnvironmentVariable(Constants.NumberOfPagesToCrawl));
        var page = await Context.NewPageAsync().ConfigureAwait(false);
        await page.GotoAsync($"https://www.ah.nl/zoeken?query={System.Web.HttpUtility.UrlEncode(searchTerm)}&page={numberOfPagesToCrawl}", new PageGotoOptions { Timeout = 0 }).ConfigureAwait(false);
        var productElements = await page.QuerySelectorAllAsync("//*[@data-testhook='product-card']").ConfigureAwait(false);

        foreach (var productElement in productElements)
        {
            try
            {
                using Task<IElementHandle?> getProductTitle = productElement.QuerySelectorAsync("css=[data-testhook='product-title']");
                using Task<IElementHandle?> getPriceUnits = productElement.QuerySelectorAsync("css=[class='price-amount_integer__1cJgL']");
                using Task<IElementHandle?> getPriceFraction = productElement.QuerySelectorAsync("css=[class='price-amount_fractional__2wVIK']");
                using Task<IElementHandle?> getUnitSize = productElement.QuerySelectorAsync("css=[data-testhook='product-unit-size']");
                using Task<IElementHandle?> getImageSource = productElement.QuerySelectorAsync("css=[data-testhook='product-image']");
                using Task<IElementHandle?> getDetailsPageUrl = productElement.QuerySelectorAsync("css=a:first-child");

                await Task.WhenAll(getImageSource, getPriceFraction, getUnitSize, getPriceUnits, getProductTitle, getDetailsPageUrl);

                IElementHandle? productTitle = await getProductTitle.ConfigureAwait(false);
                IElementHandle? productPriceUnits = await getPriceUnits.ConfigureAwait(false);
                IElementHandle? productPriceFraction = await getPriceFraction.ConfigureAwait(false);
                IElementHandle? unitSize = await getUnitSize.ConfigureAwait(false);
                IElementHandle? imageElement = await getImageSource.ConfigureAwait(false);
                IElementHandle? detailsPageLink = await getDetailsPageUrl.ConfigureAwait(false);

                string? detailsPageUrl = await detailsPageLink.GetAttributeAsync("href").ConfigureAwait(false);
                string? id = detailsPageUrl?.Split("/")[3] ?? "";
                string? scrapedImageSource = await imageElement.GetAttributeAsync("src").ConfigureAwait(false);
                string scrapedPriceUnits = await productPriceUnits.TextContentAsync().ConfigureAwait(false) ?? "";
                string scrapedPriceFraction = await productPriceFraction.TextContentAsync().ConfigureAwait(false) ?? "";
                string scrapedTitle = await productTitle.TextContentAsync().ConfigureAwait(false) ?? "";
                string scrapedUnitSize = "";
                string scrapedUnitOfMeasure = "";

                if (unitSize is not null)
                {
                    Console.WriteLine(await unitSize.TextContentAsync().ConfigureAwait(false));
                    var scrapedUnitSizeParts = (await unitSize.TextContentAsync().ConfigureAwait(false))?.Split(" ");
                    scrapedUnitSize = scrapedUnitSizeParts[0] is not null ? scrapedUnitSizeParts[0] : "";
                    scrapedUnitOfMeasure = scrapedUnitSizeParts[1] is not null ? scrapedUnitSizeParts[1] : "";
                }

                await using var detailsContext = await Browser.NewContextAsync(new BrowserNewContextOptions { }).ConfigureAwait(false);
                var detailsPage = await detailsContext.NewPageAsync().ConfigureAwait(false);
                await detailsPage.GotoAsync($"https://www.ah.nl{detailsPageUrl}", new PageGotoOptions { });
                var descriptionElement = await detailsPage.QuerySelectorAsync("css=[data-testhook='product-summary']");
                var scrapedDescription = await descriptionElement.TextContentAsync() ?? string.Empty;
                await detailsContext.CloseAsync().ConfigureAwait(false);

                var validatedProduct = Product.Create(id, scrapedTitle, scrapedDescription, merchantName, scrapedPriceUnits, scrapedPriceFraction, scrapedImageSource, scrapedUnitSize, scrapedUnitOfMeasure);
                switch (validatedProduct)
                {
                    case Valid<Product>(var product):
                        var result = IndexDocumentsAction.MergeOrUpload(product.ToIndexableProduct());
                        await SearchClient.IndexDocumentsAsync(IndexDocumentsBatch.Create(result));
                        break;
                    case Invalid<Product>(var errors):
                        // TODO : proper tracing
                        foreach (var error in errors)
                        {
                            Console.WriteLine(error);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }

        await page.CloseAsync().ConfigureAwait(false);
    }


}

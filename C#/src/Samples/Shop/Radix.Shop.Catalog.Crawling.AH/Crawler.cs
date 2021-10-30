using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Radix.Shop.Catalog.Domain;
using Microsoft.Playwright;
using Azure.Search.Documents.Models;
using Radix.Shop.Catalog.Search.Index;
using Radix.Shop.Catalog.Search;
using System;

namespace Radix.Shop.Catalog.Crawling.AH;


public class Crawler
{

    public static IBrowser? Browser { get; set; }
    public static IBrowserContext? Context { get; private set; }

    [FunctionName("crawl")]
    public static async Task Run([QueueTrigger("stq-radix-samples-shop-catalog-ah-searchterms", Connection = "AH_CONNECTION_STRING")]string searchTerm, ILogger log)
    {
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

        foreach (var productElement in productElements)
        {
            using Task<IElementHandle?> getProductTitle = productElement.QuerySelectorAsync("css=[data-testhook='product-title']");
            using Task<IElementHandle?> getPriceUnits = productElement.QuerySelectorAsync("css=[class='price-amount_integer__1cJgL']");
            using Task<IElementHandle?> getPriceFraction = productElement.QuerySelectorAsync("css=[class='price-amount_fractional__2wVIK']");
            using Task<IElementHandle?> getUnitSize = productElement.QuerySelectorAsync("css=[data-testhook='product-unit-size']");
            using Task<IElementHandle?> getImageSource = productElement.QuerySelectorAsync("css=[data-testhook='product-image']");
            using Task<IElementHandle?> getDetailsPageUrl = productElement.QuerySelectorAsync("css=a:first-child");

            await Task.WhenAll(getImageSource, getPriceFraction, getUnitSize, getPriceUnits, getProductTitle, getDetailsPageUrl);

            var productTitle = await getProductTitle.ConfigureAwait(false);
            var productPriceUnits = await getPriceUnits.ConfigureAwait(false);
            var productPriceFraction = await getPriceFraction.ConfigureAwait(false);
            var unitSize = await getUnitSize.ConfigureAwait(false);
            var imageElement = await getImageSource.ConfigureAwait(false);
            var imageSource = await imageElement.GetAttributeAsync("src").ConfigureAwait(false);
            var detailsPageLink = await getDetailsPageUrl.ConfigureAwait(false);
            var detailsPageUrl = await detailsPageLink.GetAttributeAsync("href").ConfigureAwait(false);
            var id = detailsPageUrl?.Split("/")[3] ?? "";

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

            await using var detailsContext = await Browser.NewContextAsync(new BrowserNewContextOptions { }).ConfigureAwait(false);
            var detailsPage = await detailsContext.NewPageAsync().ConfigureAwait(false);
            await detailsPage.GotoAsync($"https://www.ah.nl{detailsPageUrl}", new PageGotoOptions { });
            var descriptionElement = await detailsPage.QuerySelectorAsync("css=[data-testhook='product-summary']");
            var description = await descriptionElement.TextContentAsync() ?? string.Empty;
            await  detailsContext.CloseAsync().ConfigureAwait(false);

            var product =
                new Product
                {
                    Id = id,
                    Title = (ProductTitle)scrapedProductTitle,
                    Description = (ProductDescription)description,
                    ImageSource = (ProductImageUri)imageSource,
                    MerchantName = (MerchantName)merchant,
                    PriceUnits = (PriceUnits)int.Parse(scrapedPriceUnits),
                    PriceFraction = (PriceFraction)int.Parse(scrapedPriceFraction),
                    UnitSize = scrapedUnitSize,
                    UnitOfMeasure = scrapedUnitOfMeasure
                };

            IndexDocumentsAction.Upload(product.ToIndexableProduct());
        }

        await page.CloseAsync();          
    }
}

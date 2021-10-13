using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Radix.Shop.Catalog.Domain;
using Microsoft.Playwright;
using System.Collections.Generic;
using Radix.Result;
using static Radix.Result.Extensions;
using System.Linq;
using Microsoft.Extensions.ObjectPool;

namespace Radix.Shop.Catalog.Search.AH;


public static class Search
{

    private static IPlaywright s_playwright;
    private static IBrowser s_browser;

    [FunctionName("Search")]
    public static async IAsyncEnumerable<ProductDTO> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {

        if (s_playwright is null) s_playwright = await Playwright.CreateAsync();
        if (s_browser is null) s_browser = await s_playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { });
        var context = await s_browser.NewContextAsync(new BrowserNewContextOptions { IgnoreHTTPSErrors = true });
        const string merchant = "Albert Heijn";

        await context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true
        });

        var page = await context.NewPageAsync();
        await page.GotoAsync($"https://www.ah.nl/zoeken?query={System.Web.HttpUtility.UrlEncode(req.Query["searchTerm"])}");

        var productElements = await page.QuerySelectorAllAsync("//*[@data-testhook='product-card']");

        foreach (var productElement in productElements)
        {
            Task<IElementHandle> getProductTitle = productElement.QuerySelectorAsync("css=[data-testhook='product-title']");
            Task<IElementHandle> getPriceUnits = productElement.QuerySelectorAsync("css=[class='price-amount_integer__1cJgL']");
            Task<IElementHandle> getPriceFraction = productElement.QuerySelectorAsync("css=[class='price-amount_fractional__2wVIK']");
            Task<IElementHandle> getUnitSize = productElement.QuerySelectorAsync("css=[data-testhook='product-unit-size']");
            Task<IElementHandle> getImageSource = productElement.QuerySelectorAsync("css=[data-testhook='product-image']");

            await Task.WhenAll(getImageSource, getPriceFraction, getUnitSize, getPriceUnits, getProductTitle);

            var productTitle = await getProductTitle;
            var productPriceUnits = await getPriceUnits;
            var productPriceFraction = await getPriceFraction;
            var unitSize = await getUnitSize;
            var imageElement = await getImageSource;
            var imageSource = await imageElement.GetAttributeAsync("src");

            string scrapedPriceUnits = await productPriceUnits.TextContentAsync() ?? "";
            string scrapedPriceFraction = await productPriceFraction.TextContentAsync() ?? "";
            string scrapedProductTitle = await productTitle.TextContentAsync() ?? "";
            string scrapedUnitSize = "";
            string scrapedUnitOfMeasure = "";

            if (unitSize is not null)
            {
                var scrapedUnitSizeParts = (await unitSize.TextContentAsync()).Split(" ");
                scrapedUnitSize = scrapedUnitSizeParts[0];
                scrapedUnitOfMeasure = scrapedUnitSizeParts[1];
            }

            var productDTO =
                new ProductDTO
                {
                    Title = scrapedProductTitle,
                    Description = "",
                    ImageSource = imageSource,
                    MerchantName = merchant,
                    PriceUnits = scrapedPriceUnits,
                    PriceFraction = scrapedPriceFraction,
                    UnitSize = scrapedUnitSize,
                    UnitOfMeasure = scrapedUnitOfMeasure
                };

            yield return productDTO;
        }

        await page.CloseAsync();

        // Stop tracing and export it into a zip archive.
        await context.Tracing.StopAsync(new TracingStopOptions
        {
            Path = "trace.zip"
        });            
    }
}

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

namespace Radix.Shop.Catalog.Search.AH;

public static class Search
{
    [FunctionName("Search")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions {  });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions { IgnoreHTTPSErrors = true });
        const string merchant = "Albert Heijn";

        await context.Tracing.StartAsync(new TracingStartOptions
        {
            Screenshots = true,
            Snapshots = true
        });

        var page = await context.NewPageAsync();
        await page.GotoAsync($"https://www.ah.nl/zoeken?query={System.Web.HttpUtility.UrlEncode(req.Query["searchTerm"])}");

        var productElements = await page.QuerySelectorAllAsync("//*[@data-testhook='product-card']");
        

        var products = new List<ProductDTO>();
        var tasks = productElements.Select(productElement => ScrapeProduct(productElement, merchant, products));

        await Task.WhenAll(tasks);

        await page.CloseAsync();

        // Stop tracing and export it into a zip archive.
        await context.Tracing.StopAsync(new TracingStopOptions
        {
            Path = "trace.zip"
        });

        return new OkObjectResult(products);

        static async Task ScrapeProduct(IElementHandle productElement, string merchant, List<ProductDTO> products)
        {
            Task<IElementHandle> getProductTitle = productElement.QuerySelectorAsync("css=[data-testhook='product-title']");
            Task<IElementHandle> getPriceUnits = productElement.QuerySelectorAsync("css=[class='price-amount_integer__1cJgL']");
            Task<IElementHandle> getPriceFraction = productElement.QuerySelectorAsync("css=[class='price-amount_fractional__2wVIK']");
            Task<IElementHandle> getUnitSize = productElement.QuerySelectorAsync("css=[data-testhook='product-unit-size']");
            Task<IElementHandle> getImageSource = productElement.QuerySelectorAsync("css=[data-testhook='product-image']");

            // await Task.WhenAll(getImageSource, getPriceFraction, getUnitSize, getPriceUnits, getProductTitle);

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

            var product =
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
            products.Add(product);
        }
    }
}

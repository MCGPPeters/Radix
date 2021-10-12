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
        var page = await context.NewPageAsync();
        await page.GotoAsync($"https://www.ah.nl/zoeken?query={System.Web.HttpUtility.UrlEncode(req.Query["searchTerm"])}");

        var productElements = await page.QuerySelectorAllAsync("//*[@data-testhook='product-card']");

        var products = new List<ProductDTO>();

        foreach (var productElement in productElements)
        {
            var productTitle = await productElement.QuerySelectorAsync("css=[data-testhook='product-title']");
            var productPriceUnits = await productElement.QuerySelectorAsync("css=[class='price-amount_integer__1cJgL']");
            var productPriceFraction = await productElement.QuerySelectorAsync("css=[class='price-amount_fractional__2wVIK']");
            var productUnitSize = await productElement.QuerySelectorAsync("css=[data-testhook='product-unit-size']");
            var imageElement = await productElement.QuerySelectorAsync("css=[data-testhook='product-image']");
            var imageSource = await imageElement.GetAttributeAsync("src"); 

            string scrapedPriceUnits = await productPriceUnits.TextContentAsync();
            string scrapedPriceFraction = await productPriceFraction.TextContentAsync();
            string scrapedPoductTitle = await productTitle.TextContentAsync();
            var product = new ProductDTO
                                {
                                    Title = scrapedPoductTitle,
                                    Description = "",
                                    ImageSource = imageSource,
                                    MerchantName = merchant,
                                    PriceUnits = scrapedPriceUnits,
                                    PriceFraction = scrapedPriceFraction
                                };
            products.Add(product);
        }

        await page.CloseAsync();

        return new OkObjectResult(products);
    }
}

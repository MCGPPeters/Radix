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
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log)
    {

        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions {  });
        var context = await browser.NewContextAsync(new BrowserNewContextOptions { IgnoreHTTPSErrors = true });
        const string merchant = "Albert Heijn";
        var page = await context.NewPageAsync();
        await page.GotoAsync($"https://www.ah.nl/zoeken?query={System.Web.HttpUtility.UrlEncode(req.Query["searchTerm"])}");

        var productElements = await page.QuerySelectorAllAsync("//*[@data-testhook='product-card']");

        var products = new List<Product>();

        foreach (var productElement in productElements)
        {
            var productTitle = await productElement.QuerySelectorAsync("css=[data-testhook='product-title']");
            var productPriceUnits = await productElement.QuerySelectorAsync("css=[class='price-amount_integer__1cJgL']");
            var productPriceFraction = await productElement.QuerySelectorAsync("css=[class='price-amount_fractional__2wVIK']");
            var productUnitSize = await productElement.QuerySelectorAsync("css=[data-testhook='product-unit-size']");

            string scrapedPriceUnits = await productPriceUnits.TextContentAsync();
            string scrapedPriceFraction = await productPriceFraction.TextContentAsync();
            string scrapedPoductTitle = await productTitle.TextContentAsync();
            var productResult = from parsedPriceUnits in Math.Pure.Numbers.ℤ.FromString.Parse(scrapedPriceUnits)
                                from parsedPriceFraction in Math.Pure.Numbers.ℤ.FromString.Parse(scrapedPriceFraction)
                                select new Product(
                                        (ProductTitle)scrapedPoductTitle,
                                        (ProductDescription)"",
                                        (ProductImageUri)"",
                                        (MerchantName)merchant,
                                        new Price((PriceUnits)parsedPriceUnits, (PriceFraction)parsedPriceFraction));
            switch (productResult)
            {
                case Ok<Product, Error> product:
                    products.Add(product);
                    break;
            }
        }

        await page.CloseAsync();

        return new OkObjectResult(products);
    }
}

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
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using static Radix.Writer.Extensions;
using static Radix.Async.Extensions;

namespace Radix.Shop.Catalog.Crawling.Jumbo
{
    public class Crawler
    {
        public IBrowser Browser { get; set; }
        public IBrowserContext BrowserContext { get; }
        public IConfiguration Configuration { get; }
        public SearchClient SearchClient { get; set; }

        static ActivitySource s_ActivitySource = new("AH");

        public Crawler(SearchClient searchClient, IBrowser browser, IBrowserContext browserContext, IConfiguration configuration)
        {
            SearchClient = searchClient;
            Browser = browser;
            BrowserContext = browserContext;
            Configuration = configuration;
        }

        [FunctionName("AH")]
        public async Task Run([QueueTrigger("%AH_QUEUE_NAME%", Connection = "AH_CONNECTION_STRING")]string searchTerm, ILogger log)
        {
            const string merchantName = "Albert Heijn";

            using (Activity? activity = s_ActivitySource.StartActivity($"Searching merchant {merchantName} for products containing search term '{searchTerm}'")) ;

            int numberOfPagesToCrawl = int.Parse(Configuration[Constants.NumberOfPagesToCrawl]);
            var page = await BrowserContext.NewPageAsync().ConfigureAwait(false);
            string url = $"https://www.ah.nl/zoeken?query={System.Web.HttpUtility.UrlEncode(searchTerm)}&page={numberOfPagesToCrawl}";
            await page
                .GotoAsync(url, new PageGotoOptions { Timeout = 0 })
                .ConfigureAwait(false);
            var productElements = await page.QuerySelectorAllAsync("//*[@data-testhook='product-card']").ConfigureAwait(false);

            foreach (var productElement in productElements)
            {
                try
                {
                    // Get all data in parallel and create a validated product
                    var validatedProduct =
                        Return(CreateProduct())
                            .Apply(GetProductTitle(productElement))
                            .Apply(GetProductDescriptionAndId(productElement, Browser))
                            .Apply(Return(merchantName))
                            .Apply(GetPriceUnits(productElement))
                            .Apply(GetPriceFraction(productElement))
                            .Apply(GetImageSource(productElement))
                            .Apply(GetUnit(productElement));


                    switch (await validatedProduct)
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

        private static Task<string> GetProductTitle(IElementHandle productElement) =>
                                    from productTitleElement in productElement.QuerySelectorAsync("css=[data-testhook='product-title']")
                                    from productTitle in productTitleElement.TextContentAsync()
                                    select productTitle;

        private static Task<string> GetPriceUnits(IElementHandle productElement) =>
                                    from priceUnitElement in productElement.QuerySelectorAsync("css=[class='price-amount_integer__1cJgL']")
                                    from priceUnit in priceUnitElement.TextContentAsync()
                                    select priceUnit;

        private static Task<string> GetPriceFraction(IElementHandle productElement) =>
                                    from priceFractionElement in productElement.QuerySelectorAsync("css=[class='price-amount_fractional__2wVIK']")
                                    from priceFraction in priceFractionElement.TextContentAsync()
                                    select priceFraction;

        private static Task<(string, string)> GetUnit(IElementHandle productElement) =>
                                    from unitSizeElement in productElement.QuerySelectorAsync("css=[data-testhook='product-unit-size']")
                                    from unitSizeElementText in unitSizeElement.TextContentAsync()
                                    let unitSizeParts = unitSizeElementText.Split(" ")
                                    let unitSize = unitSizeParts[0]
                                    let unitOfMeasure = unitSizeParts[1]
                                    select (unitSize, unitOfMeasure);

        private static Task<string> GetImageSource(IElementHandle productElement) =>
                                    from imageSourceElement in productElement.QuerySelectorAsync("css=[data-testhook='product-image']")
                                    from imagesSource in imageSourceElement.GetAttributeAsync("src")
                                    select imagesSource;

        private static Task<(string id, string description)> GetProductDescriptionAndId(IElementHandle productElement, IBrowser browser) =>
                                    from detailsPageElement in productElement.QuerySelectorAsync("css=a:first-child")
                                    from detailsPageLink in detailsPageElement.GetAttributeAsync("href")
                                    from detailsContext in browser.NewContextAsync(new BrowserNewContextOptions { })
                                    from detailsPage in detailsContext.NewPageAsync()
                                    from _ in detailsPage.GotoAsync($"https://www.ah.nl{detailsPageLink}", new PageGotoOptions { Timeout = 0 })
                                    from descriptionElement in detailsPage.QuerySelectorAsync("css=[data-testhook='product-summary']")
                                    from description in descriptionElement.TextContentAsync()
                                    select (detailsPageLink?.Split("/")[3] ?? "", description ?? "");

        private static Func<string, (string id, string description), string, string, string, string, (string unitSize, string unitOfMeasure), Validated<Product>> CreateProduct() =>
                                (title, idAndDescription, merchantName, priceUnits, priceFraction, imageSource, unitSizeAndUnitOfMeasure) =>
                                    Product.Create(idAndDescription.id, title, idAndDescription.description, merchantName, priceUnits, priceFraction, imageSource, unitSizeAndUnitOfMeasure.unitSize, unitSizeAndUnitOfMeasure.unitOfMeasure);
    }
}

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Radix.Shop.Catalog.Domain;
using Microsoft.Playwright;
using Azure.Search.Documents.Models;
using Radix.Shop.Catalog.Search.Index;
using Radix.Shop.Catalog.Search;
using Azure.Search.Documents;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using static Radix.Control.Task.Result.Extensions;
using static Radix.Control.Result.Extensions;
using Radix.Control.Option;
using Radix.Data.Collections.Generic.Enumerable;
using Radix.PlayWright.ElementHandle;
using Radix.PlayWright.Browser;
using Radix.PlayWright.BrowserContext;
using Radix.PlayWright.Page;

namespace Radix.Shop.Catalog.Crawling.AH
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
        public async Task Run([QueueTrigger("%AH_QUEUE_NAME%", Connection = "AH_CONNECTION_STRING")] string searchTerm, ILogger log)
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
                        CreateProduct()
                            .Apply(GetProductTitle(productElement))
                            .Apply(GetProductDescriptionAndId(productElement, Browser))
                            .Apply(Return<string, Error>(merchantName))
                            .Apply(GetPriceUnits(productElement))
                            .Apply(GetPriceFraction(productElement))
                            .Apply(GetImageSource(productElement))
                            .Apply(GetUnit(productElement));


                    switch (await validatedProduct)
                    {
                        case Ok<Product, Error>(var product):
                            var result = IndexDocumentsAction.MergeOrUpload(product.ToIndexableProduct());
                            await SearchClient.IndexDocumentsAsync(IndexDocumentsBatch.Create(result)).ConfigureAwait(false);
                            break;
                        case Error<Product, Error>(var error):
                            Console.WriteLine($"Validation error : {error}");
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

        private static Task<Result<string, Error>> GetProductTitle(IElementHandle productElement) =>
                                    from productTitleElement in productElement.QuerySelector("css=[data-testhook='product-title']")
                                    from productTitle in productTitleElement.TextContent()
                                    select productTitle;

        private static Task<Result<string, Error>> GetPriceUnits(IElementHandle productElement) =>
                                    from priceUnitElement in productElement.QuerySelector("css=[class='price-amount_integer__1cJgL']")
                                    from priceUnit in priceUnitElement.TextContent()
                                    select priceUnit;

        private static Task<Result<string, Error>> GetPriceFraction(IElementHandle productElement) =>
                                    from priceFractionElement in productElement.QuerySelector("css=[class='price-amount_fractional__2wVIK']")
                                    from priceFraction in priceFractionElement.TextContent()
                                    select priceFraction;

        private static Task<Result<(string unitSize, string unitOfMeasure), Error>> GetUnit(IElementHandle productElement) =>
                                    from unitSizeElement in productElement.QuerySelector("css=[data-testhook='product-unit-size']")
                                    from unitSizeElementText in unitSizeElement.TextContent()
                                    let unitSizeParts = unitSizeElementText.Split(" ")
                                    let unitSize = unitSizeParts[0]
                                    let unitOfMeasure = unitSizeParts[1]
                                    select (unitSize, unitOfMeasure);

        private static Task<Result<string, Error>> GetImageSource(IElementHandle productElement) =>
                                    from imageSourceElement in productElement.QuerySelector("css=[data-testhook='product-image']")
                                    from imagesSource in imageSourceElement.GetAttribute("src")
                                    select imagesSource;

        private static Task<Result<(string id, string description), Error>> GetProductDescriptionAndId(IElementHandle productElement, IBrowser browser) =>
                                    from detailsPageElement in productElement.QuerySelector("css=a:first-child")
                                    from detailsPageLink in detailsPageElement.GetAttribute("href")
                                    from detailsContext in browser.NewContext(new BrowserNewContextOptions { })
                                    from detailsPage in detailsContext.NewPage()
                                    from _ in detailsPage.Goto($"https://www.ah.nl{detailsPageLink}", new PageGotoOptions { Timeout = 0 })
                                    from descriptionElement in detailsPage.QuerySelector("css=[data-testhook='product-summary']")
                                    from description in descriptionElement.TextContent()
                                    let id = detailsPageLink?.Split("/")[3]
                                    select (id, description);

        private static Task<Func<Result<string, Error>, Result<(string id, string description), Error>, Result<string, Error>, Result<string, Error>, Result<string, Error>, Result<string, Error>, Result<(string unitSize, string unitOfMeasure), Error>, Result<Product, Error>>> CreateProduct() =>
                                Task.FromResult ((Result<string, Error> title, Result<(string id, string description), Error> idAndDescription, Result<string, Error> merchantName, Result<string, Error> priceUnits, Result<string, Error> priceFraction, Result<string, Error> imageSource, Result<(string unitSize, string unitOfMeasure), Error> unitSizeAndUnitOfMeasure) =>
                                {
                                    var id = idAndDescription.Map(x => x.id);
                                    var description = idAndDescription.Map(y => y.description);
                                    var unitSize = unitSizeAndUnitOfMeasure.Map(x => x.unitSize);
                                    var unitOfMeasure = unitSizeAndUnitOfMeasure.Map(y => y.unitOfMeasure);

                                    var result = Ok<Func<string, string, string, string, string, string, string, string, string, Validated<Product>>, Error>(Product.Create)
                                    .Apply(id)
                                    .Apply(title)
                                    .Apply(description)
                                    .Apply(merchantName)
                                    .Apply(priceUnits)
                                    .Apply(priceFraction)
                                    .Apply(imageSource)
                                    .Apply(unitSize)
                                    .Apply(unitOfMeasure);

                                    return result switch
                                    {
                                        Ok<Validated<Product>, Error>(var validated) =>
                                            validated switch
                                            {
                                                Valid<Product>(var product) => Ok<Product, Error>(product),
                                                Invalid<Product>(var errors) => Error<Product, Error>(errors.Aggregate<string, Data.String.Concat>()),
                                                _ => throw new NotImplementedException()
                                            },
                                        Error<Validated<Product>, Error>(var error) => Error<Product, Error>(error),
                                        _ => throw new NotImplementedException()
                                    };
                                });
    }
}

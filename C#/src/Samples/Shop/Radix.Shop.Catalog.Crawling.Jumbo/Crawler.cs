using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using Radix.Data;
using Radix.Shop.Catalog.Domain;
using Radix.Shop.Catalog.Search.Index;

namespace Radix.Shop.Catalog.Crawling.Jumbo
{
    public class Crawler
    {
        public IBrowser Browser { get; set; }
        public IBrowserContext BrowserContext { get; }
        public SearchClient SearchClient { get; set; }

        public Crawler(SearchClient searchClient, IBrowser browser, IBrowserContext browserContext)
        {
            SearchClient = searchClient;
            Browser = browser;
            BrowserContext = browserContext;
        }

        [FunctionName("Jumbo")]
        public async Task Run([QueueTrigger("%JUMBO_QUEUE_NAME%", Connection = "JUMBO_CONNECTION_STRING")]string searchTerm, ILogger log)
        {
            const string merchantName = "Jumbo";
            var page = await BrowserContext.NewPageAsync().ConfigureAwait(false);
            await page.GotoAsync($"https://www.jumbo.com/zoeken?searchTerms={HttpUtility.UrlEncode(searchTerm)}", new PageGotoOptions { Timeout = 0 });
            var productElements = await page.QuerySelectorAllAsync("//*[@analytics-tag='product card']");

            foreach (var productElement in productElements)
            {

                try
                {
                    using Task<IElementHandle?> getProductTitle = productElement.QuerySelectorAsync("css=[class='title-link']");
                    using Task<IElementHandle?> getPriceUnits = productElement.QuerySelectorAsync("css=[class='whole']");
                    using Task<IElementHandle?> getPriceFraction = productElement.QuerySelectorAsync("css=[class='fractional']");
                    using Task<IElementHandle?> getUnitSizeAndMeasure = productElement.QuerySelectorAsync("css=[class='price-per-unit']");
                    using Task<IElementHandle?> getImageSource = productElement.QuerySelectorAsync("css=[class='image']");
                    using Task<IElementHandle?> getDetailsPageUrl = productElement.QuerySelectorAsync("css=a:first-child");

                    await Task.WhenAll(getImageSource, getPriceFraction, getUnitSizeAndMeasure, getPriceUnits, getProductTitle, getDetailsPageUrl);

                    IElementHandle? productTitleHandle = await getProductTitle.ConfigureAwait(false);
                    IElementHandle? productPriceUnitsHandle = await getPriceUnits.ConfigureAwait(false);
                    IElementHandle? productPriceFractionHandle = await getPriceFraction.ConfigureAwait(false);
                    IElementHandle? unitSizeAndMeasureHandle = await getUnitSizeAndMeasure.ConfigureAwait(false);
                    IElementHandle? imageElementHandle = await getImageSource.ConfigureAwait(false);

                    string? id =  await productElement.GetAttributeAsync("data-product-Id").ConfigureAwait(false);
                    string? title = productTitleHandle is not null ? await productTitleHandle.TextContentAsync().ConfigureAwait(false) : "";
                    string? detailsPageLink = productTitleHandle is not null ? await productTitleHandle.GetAttributeAsync("href").ConfigureAwait(false) : "";
                    string? priceUnits = productPriceUnitsHandle is not null ? await productPriceUnitsHandle.TextContentAsync().ConfigureAwait(false) : "";
                    string? priceFraction = productPriceFractionHandle is not null ? await productPriceFractionHandle.TextContentAsync().ConfigureAwait(false) : "";
                    string? unitSize = "";
                    string? unitOfMeasure = "";
                    if (unitSizeAndMeasureHandle is not null)
                    {
                        var unitSizeAndMeasure = await unitSizeAndMeasureHandle.TextContentAsync().ConfigureAwait(false);
                        var parts = unitSizeAndMeasure is not null ? unitSizeAndMeasure.Split('/') : new[] { "", "" };
                        unitSize = RemoveSpecialCharacters(parts[0]);
                        unitOfMeasure = RemoveSpecialCharacters(parts[1]);
                    }
                    string? imageSource = await imageElementHandle.GetAttributeAsync("src").ConfigureAwait(false);
                    string? description = "";

                    if (detailsPageLink is not null)
                    { 
                        await using var detailsContext = await Browser.NewContextAsync(new BrowserNewContextOptions { }).ConfigureAwait(false);
                        var detailsPage = await detailsContext.NewPageAsync().ConfigureAwait(false);
                        await detailsPage.GotoAsync($"https://www.jumbo.com{detailsPageLink}", new PageGotoOptions { Timeout = 0 });
                        var descriptionElement = await detailsPage.QuerySelectorAsync("css=[analytics-tag='product description collapsible']");
                        description = descriptionElement is not null ? await descriptionElement.TextContentAsync() ?? string.Empty : "";
                        await detailsContext.CloseAsync().ConfigureAwait(false);
                    }

                    var validatedProduct = Product.Create(id, title, description, merchantName, priceUnits, priceFraction, imageSource, unitSize, unitOfMeasure);

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

            await page.CloseAsync();
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}

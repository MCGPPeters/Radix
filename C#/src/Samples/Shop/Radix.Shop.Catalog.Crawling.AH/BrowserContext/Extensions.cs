using System.Threading.Tasks;
using Microsoft.Playwright;
using Radix.Data;
using static Radix.Control.Task.Extensions;
using static Radix.Control.Result.Extensions;

namespace Radix.Shop.Catalog.Crawling.AH.BrowserContext;

public static class Extensions
{
    public static Task<Result<IPage, Error>> NewPage(this IBrowserContext context) =>
        context.NewPageAsync()
        .Map2(
            faulted =>
                Error<IPage, Error>($"Could not open a new page in the browser context because of {faulted.InnerException}"),
            succeeded =>
                Ok<IPage, Error>(succeeded));
}   


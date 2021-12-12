using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Radix.Control.Task;
using static Radix.Control.Task.Extensions;
using static Radix.Control.Result.Extensions;
using static Radix.Control.Validated.Extensions;
using Radix.Control.Result;
using Radix.Data;

namespace Radix.Shop.Catalog.Crawling.AH.Browser;

public static class Extensions
{
    public static Task<Result<IBrowserContext, Error>> NewContext(this IBrowser browser, BrowserNewContextOptions browserNewContextOptions) =>
        browser.NewContextAsync(browserNewContextOptions)
            .Map2(
                faulted =>
                    Error<IBrowserContext, Error>($"Could not open new browser context because of {faulted.InnerException}"),
                succeeded =>
                    Ok<IBrowserContext, Error>(succeeded));
}

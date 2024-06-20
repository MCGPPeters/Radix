using Microsoft.Playwright;
using Radix.Control.Task;
using static Radix.Control.Task.Extensions;
using static Radix.Control.Result.Extensions;
using Radix.Data;
using System.Threading.Tasks;

namespace Radix.PlayWright.Browser;

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

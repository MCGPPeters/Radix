using System.Threading.Tasks;
using Microsoft.Playwright;
using Radix.Data;
using static Radix.Control.Task.Extensions;
using static Radix.Control.Result.Extensions;

namespace Radix.Shop.Catalog.Crawling.AH.Page;
public static class Extensions
{
    public static Task<Result<IResponse, Error>> Goto(this IPage page, string url, PageGotoOptions pageGotoOptions) =>
        page.GotoAsync(url, pageGotoOptions)
            .Map2(
                faulted =>
                    Error<IResponse, Error>($"Could not navigate to '{url}' because of {faulted.InnerException}"),
                succeeded =>
                    Ok<IResponse, Error>(succeeded));

    public static Task<Result<IElementHandle, Error>> QuerySelector(this IPage page, string selector) =>
        from result in page.QuerySelectorAsync(selector)
        select result is not null
        ? Ok<IElementHandle, Error>(result)
        : Error<IElementHandle, Error>($"The selector '{selector}' yielded no result");
}

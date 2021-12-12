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

namespace Radix.Shop.Catalog.Crawling.AH.ElementHandle;

public static class Extensions
{
    public static Task<Result<IElementHandle, Error>> QuerySelector(this IElementHandle elementHandle, string selector) =>
        from result in elementHandle.QuerySelectorAsync(selector)
        select result is not null
        ? Ok<IElementHandle, Error>(result)
        : Error<IElementHandle, Error>($"The selector '{selector}' yielded no result");

    public static Task<Result<string, Error>> GetAttribute(this IElementHandle elementHandle, string selector) =>
        from result in elementHandle.GetAttributeAsync(selector)
        select result is not null
        ? Ok<string, Error>(result)
        : Error<string, Error>($"The selector '{selector}' yielded no result");

    public static Task<Result<string, Error>> TextContent(this IElementHandle elementHandle) =>
       from result in elementHandle.TextContentAsync()
       select result is not null
       ? Ok<string, Error>(result)
       : Error<string, Error>($"Getting text from the element yielded no result");
}



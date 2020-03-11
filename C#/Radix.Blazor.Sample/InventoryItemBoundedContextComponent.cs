using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using static Radix.Blazor.Html.Elements;
using static Radix.Blazor.Html.Attributes;
using Radix.Tests.Models;

namespace Radix.Blazor.Sample
{
    // ReSharper disable once ClassNeverInstantiated.Global : Instantiated via DI
    public class InventoryItemBoundedContextComponent : BoundedContextComponent<IndexViewModel, InventoryItemCommand, InventoryItemEvent>
    {

        protected override Node View(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext)
        {
            return h1(new[] { value("Hello world") });
        }

    }
}

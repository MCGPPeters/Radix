using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Blazor.Html;
using Radix.Monoid;
using Radix.Result;
using Radix.Tests.Models;
using Radix.Validated;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Components;
using static Radix.Blazor.Html.Elements;

namespace Radix.Blazor.Inventory.Pages
{
    [Route("/Add")]
    public class AddInventoryItemComponent : Component<AddInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent>
    {
        public override Node Render(AddInventoryItemViewModel currentViewModel)
        {

            return concat(
                h1(Enumerable.Empty<IAttribute>(), text("Add new item")),
                div(
                    new[] {@class("form-group")},
                    Elements.label(
                        new[] {@for("nameInput")},
                        text("Name")),
                    input(
                        @class("form-control"),
                        id("nameInput"),
                        on.input(
                            args => { CurrentViewModel.InventoryItemName = args.Value.ToString(); }),
                        value(currentViewModel.InventoryItemName)),
                    Elements.label(
                        new[] {@for("countInput")},
                        text("Count")),
                    input(
                        @class("form-control"),
                        id("countInput"),
                        value(CurrentViewModel.InventoryItemCount.ToString()),
                        on.input(
                            args =>
                            {
                                Console.Out.WriteLine(args.Value);
                                currentViewModel.InventoryItemCount = int.Parse(args.Value.ToString());
                            }))),
                button(
                    new[]
                    {
                        @class("btn btn-primary"),
                        on.click(
                            async args =>
                            {
                                var inventoryItem = await BoundedContext.Create<InventoryItem>();
                                var item = CreateInventoryItem.Create(currentViewModel.InventoryItemName, true, currentViewModel.InventoryItemCount);
                                switch (item)
                                {
                                    case Valid<InventoryItemCommand> (var validCommand):
                                        IVersion expectedVersion = new AnyVersion();

                                        var result = await BoundedContext.Send<InventoryItem>(inventoryItem, validCommand, expectedVersion);
                                        switch (result)
                                        {
                                            case Ok<InventoryItemEvent[], Error[]>(var events):
                                                currentViewModel.Apply(events);

                                                break;
                                            case Error<InventoryItemEvent[], Error[]>(var errors):
                                                Console.Out.WriteLine($"errors : {errors}");
                                                currentViewModel.Errors = errors.ToList();
                                                if (JSRuntime is object)
                                                    await JSRuntime.InvokeAsync<string>("$('.toast').toast(option)", Array.Empty<object>());
                                                break;
                                        }

                                        break;
                                    case Invalid<InventoryItemCommand> invalidCommand:
                                        Console.Out.WriteLine($"invalid : {invalidCommand.Reasons.Aggregate((s1, s2) => s1 + " " + s2)}");
                                        // OnNext(currentViewModel);
                                        if (JSRuntime is object)
                                            await JSRuntime.InvokeAsync<string>("$('.toast').toast(option)", Array.Empty<object>());
                                        break;
                                    default:
                                        throw new InvalidOperationException();
                                }

                            })
                    },
                    text("Ok")
                ),
                navLinkMatchAll(new[] {@class("btn btn-primary"), href("/")}, text("Cancel")),
                currentViewModel.Errors.Any()
                    ? div(
                        Enumerable.Empty<IAttribute>(),
                        div(
                            new[] {@class("toast"), attribute("data-autohide", "true")},
                            div(
                                new[] {@class("toast-header")},
                                img(new[] {src("..."), @class("rounded-mr2"), alt("...")}),
                                strong(new[] {@class("mr-auto")}, text("Invalid input")),
                                small(Array.Empty<IAttribute>(), text(DateTimeOffset.UtcNow.ToString()))),
                            div(
                                new[] {@class("toast-body")},
                                FormatErrorMessages(currentViewModel.Errors)
                            )))
                    : empty);
        }

        private Node FormatErrorMessages(IEnumerable<Error> errors)
        {
            Node node = new Empty();
            if (errors is object)
                node = ul(Array.Empty<IAttribute>(), errors.Select(error => li(Array.Empty<IAttribute>(), text(error.ToString()))).ToArray());

            return node;
        }
    }
}

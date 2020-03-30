using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radix.Blazor.Html;
using Radix.Monoid;
using Radix.Result;
using Radix.Tests.Models;
using Radix.Validated;
using static Radix.Blazor.Html.Attributes;
using static Radix.Blazor.Html.Components;
using static Radix.Blazor.Html.Elements;

namespace Radix.Blazor.Sample.Components
{
    public class AddInventoryItemComponent : Component<AddInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent>
    {


        public AddInventoryItemComponent(BoundedContext<InventoryItemCommand, InventoryItemEvent> boundedContext,
            ReadModel<AddInventoryItemViewModel, InventoryItemEvent> readModel, IJSRuntime jsRuntime) : base(boundedContext, readModel, jsRuntime)
        {
        }

        public override Node Render(AddInventoryItemViewModel currentViewModel)
        {
            return concat(
                h1(Enumerable.Empty<IAttribute>(), text("Add new item")),
                Elements.form(
                    new[] {@class("needs-validation"), novalidate()},
                    div(
                        new[] {@class("form-group")},
                        Elements.label(
                            new[] {@for("nameInput")},
                            text("Name")),
                        input(
                            @class("form-control"),
                            id("nameInput"),
                            on.change(
                                args =>
                                {
                                    currentViewModel.InventoryItemName = args.Value.ToString();
                                    return Task.CompletedTask;
                                })),
                        Elements.label(
                            new[] {@for("countInput")},
                            text("Count")),
                        input(@class("form-control"), id("countInput"), value(currentViewModel.InventoryItemCount.ToString()))),
                    button(
                        new[]
                        {
                            @class("btn btn-primary"),
                            on.click(
                                async args =>
                                {
                                    var inventoryItem = await BoundedContext.Create<InventoryItem>();
                                    var item = CreateInventoryItem.Create(currentViewModel.InventoryItemName, true, currentViewModel.InventoryItemCount);
                                    var command = Command<InventoryItemCommand>.Create(() => item);
                                    ;
                                    switch (command)
                                    {
                                        case Valid<Command<InventoryItemCommand>> validCommand:
                                            IVersion expectedVersion = new AnyVersion();
                                            var result = await BoundedContext.Send<InventoryItem>(
                                                new CommandDescriptor<InventoryItemCommand>(inventoryItem, validCommand, expectedVersion));
                                            switch (result)
                                            {
                                                case Ok<InventoryItemEvent[], Error[]>(var events):
                                                    OnNext(currentViewModel.Apply(events));
                                                    break;
                                                case Error<InventoryItemEvent[], Error[]>(var errors):
                                                    currentViewModel.Errors = errors;
                                                    OnNext(currentViewModel);
                                                    if (JSRuntime is object)
                                                        await JSRuntime.InvokeAsync<string>("$('.toast').toast(option)", Array.Empty<object>());
                                                    break;
                                            }

                                            break;
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
                        : empty));
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

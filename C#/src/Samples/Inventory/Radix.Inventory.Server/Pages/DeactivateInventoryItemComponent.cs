using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Components;
using Radix.Components.Html;
using Radix.Option;
using Radix.Inventory.Domain;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;
using static Radix.Components.Html.Components;

namespace Radix.Blazor.Inventory.Server.Pages
{
    [Route("/Deactivate/{Address:guid}")]
    public class DeactivateInventoryItemComponent : Component<DeactivateInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent, Json>
    {
        [Parameter]
        public long Id { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ViewModel.InventoryItemName = ViewModel.InventoryItems.FirstOrDefault(tuple => tuple.id == Id).Name;
        }

        protected override Update<DeactivateInventoryItemViewModel, InventoryItemEvent> Update { get; } = (state, events) => state;

        protected override Node View(DeactivateInventoryItemViewModel currentViewModel) => concat(
            h1(NoAttributes(), text($"Deactivate item : {ViewModel.InventoryItemName}")),
            div(
                new[] { @class("form-group") },
                Elements.label(
                    new[] { @for("reasonInput") },
                    text("Reason")),
                input(
                    @class("form-control"),
                    id("reasonInput"),
                    bind.input(currentViewModel.Reason, reason => currentViewModel.Reason = reason)),
                
            button(
                new[]
                {
                    @class("btn btn-primary"), on.click(
                        async args =>
                        {
                            Validated<InventoryItemCommand> validCommand = DeactivateInventoryItem.Create(currentViewModel.Reason);

                            Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = BoundedContext.Create(InventoryItem.Decide, InventoryItem.Update);
                            Option<Radix.Error[]> result = await Dispatch(inventoryItem, validCommand);
                            switch (result)
                            {
                                case Some<Radix.Error[]>(_):
                                    if (JSRuntime is object)
                                    {
                                        await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                                    }

                                    break;
                                case None<Radix.Error[]> _:
                                    NavigationManager.NavigateTo("/");
                                    break;
                            }
                        })
                },
                text("Ok")
            ),
            navLinkMatchAll(new[] { @class("btn btn-primary"), href("/") }, text("Cancel")),
            div(
                NoAttributes(),
                div(
                    new[] { @class("toast"), attribute("data-autohide", "false") },
                    div(
                        new[] { @class("toast-header") },
                        strong(new[] { @class("mr-auto") }, text("Invalid input")),
                        small(NoAttributes(), text(DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture))),
                        button(new[] { type("button"), @class("ml-2 mb-1 close"), attribute("data-dismiss", "toast") }, Elements.span(NoAttributes(), text("🗙")))),
                    div(
                        new[] { @class("toast-body") },
                        FormatErrorMessages(currentViewModel.Errors)
                    )))));

        private static IEnumerable<IAttribute> NoAttributes()
        {
            return Enumerable.Empty<IAttribute>();
        }
        private static Node FormatErrorMessages(IEnumerable<Radix.Error> errors)
        {
            Node node = new Empty();
            if (errors is object)
            {
                node = ul(Array.Empty<IAttribute>(), errors.Select(error => li(Array.Empty<IAttribute>(), text(error.ToString()))).ToArray());
            }

            return node;
        }

    }

    public record DeactivateInventoryItemViewModel : ViewModel
    {
        public List<(long id, string Name)> InventoryItems { get; }

        public DeactivateInventoryItemViewModel(List<(long id, string Name)> inventoryItems) => InventoryItems = inventoryItems;

        public IEnumerable<Radix.Error> Errors { get; set; } = new List<Radix.Error>();
        public string? InventoryItemName { get; set; }
        public string Reason { get; set; }
    }
}

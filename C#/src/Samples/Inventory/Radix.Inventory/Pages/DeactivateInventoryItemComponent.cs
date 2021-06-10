using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Radix.Components;
using Radix.Components.Html;
using Radix.Inventory.Domain;
using Radix.Option;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;
using static Radix.Components.Html.Components;

namespace Radix.Blazor.Inventory.Server.Pages
{
    [Route("/Deactivate/{id:long}")]
    public class DeactivateInventoryItemComponent : Component<DeactivateInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent, Json>
    {
        [Parameter]public long Id { get; set; }

        protected override Update<DeactivateInventoryItemViewModel, InventoryItemEvent> Update { get; } = (state, events) =>
        {
            return events.Aggregate(
                state,
                (model, @event) =>
                {
                    switch (@event)
                    {
                        case InventoryItemCreated inventoryItemCreated_:
                            break;
                        case InventoryItemDeactivated inventoryItemDeactivated:
                            state.InventoryItems = state.InventoryItems
                                .Select(item => (item.id, item.name, false))
                                .Where(tuple => tuple.id.Equals(inventoryItemDeactivated.Id)).ToList();
                            break;
                        case InventoryItemRenamed _:
                            break;
                        case ItemsCheckedInToInventory _:
                            break;
                        case ItemsRemovedFromInventory _:
                            break;
                        default:
                            throw new NotSupportedException("Unknown event");
                    }

                    return state;
                });
        };

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ViewModel.InventoryItemName = ViewModel.InventoryItems.FirstOrDefault(tuple => tuple.id == Id).name;
        }
        
        protected override Node View(DeactivateInventoryItemViewModel currentViewModel) =>
            concat(
                h1(NoAttributes(), text($"Deactivate item : {ViewModel.InventoryItemName}")),
                div(
                    new[] {@class("form-group")},
                    Elements.label(
                        new[] {@for("reasonInput")},
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
                                    Validated<InventoryItemCommand> validCommand = DeactivateInventoryItem.Create(Id, currentViewModel.Reason);

                                    Aggregate<InventoryItemCommand, InventoryItemEvent> inventoryItem = BoundedContext.Create(InventoryItem.Decide, InventoryItem.Update);
                                    Option<Error[]> result = await Dispatch(inventoryItem, validCommand);
                                    switch (result)
                                    {
                                        case Some<Error[]>(_):
                                            if (JSRuntime is not null)
                                            {
                                                await JSRuntime.InvokeAsync<string>("toast", Array.Empty<object>());
                                            }

                                            break;
                                        case None<Error[]> _:
                                            NavigationManager.NavigateTo("/");
                                            break;
                                    }
                                })
                        },
                        text("Ok")
                    ),
                    navLinkMatchAll(new[] {@class("btn btn-primary"), href("/")}, text("Cancel")),
                    div(
                        NoAttributes(),
                        div(
                            new[] {@class("toast"), attribute("data-autohide", "false")},
                            div(
                                new[] {@class("toast-header")},
                                strong(new[] {@class("mr-auto")}, text("Invalid input")),
                                small(NoAttributes(), text(DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture))),
                                button(new[] {type("button"), @class("ml-2 mb-1 close"), attribute("data-dismiss", "toast")}, Elements.span(NoAttributes(), text("🗙")))),
                            div(
                                new[] {@class("toast-body")},
                                FormatErrorMessages(currentViewModel.Errors)
                            )))));

        private static IEnumerable<IAttribute> NoAttributes()
            => Enumerable.Empty<IAttribute>();

        private static Node FormatErrorMessages(IEnumerable<Error> errors)
        {
            Node node = new Empty();
            if (errors is not null)
            {
                node = ul(Array.Empty<IAttribute>(), errors.Select(error => li(Array.Empty<IAttribute>(), text(error.ToString()))).ToArray());
            }

            return node;
        }
    }

    public record DeactivateInventoryItemViewModel : ViewModel
    {

        public DeactivateInventoryItemViewModel(List<(long id, string name, bool activated)> inventoryItems) => InventoryItems = inventoryItems;

        public List<(long id, string name, bool activated)> InventoryItems { get; set; }

        public string? InventoryItemName { get; set; }
        public string Reason { get; set; }
    }
}

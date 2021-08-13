using System.Globalization;
using Microsoft.AspNetCore.Components;
using Radix.Components;
using Radix.Components.Html;
using Radix.Inventory.Domain;
using Radix.Option;
using static Radix.Components.Html.Attributes;
using static Radix.Components.Html.Components;
using static Radix.Components.Html.Elements;

namespace Radix.Blazor.Inventory.Server.Pages;

[Route("/Deactivate/{id:guid}")]
public class DeactivateInventoryItemComponent : TaskBasedComponent<DeactivateInventoryItemViewModel, InventoryItemCommand, InventoryItemEvent, Json>
{
    [Parameter] public Guid Id { get; set; }

    protected override Node View(DeactivateInventoryItemViewModel currentViewModel) =>
        concat(
            h1(None, text($"Deactivate item : {ViewModel.InventoryItemName}")),
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
                                    Aggregate<InventoryItemCommand, InventoryItemEvent>? inventoryItem = BoundedContext.Get(Id, InventoryItem.Decide, InventoryItem.Update);
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
                navLinkMatchAll(new[] { @class("btn btn-primary"), href("/") }, text("Cancel")),
                div(
                    None,
                    div(
                        new[] { @class("toast"), attribute("data-autohide", "false") },
                        div(
                            new[] { @class("toast-header") },
                            strong(new[] { @class("mr-auto") }, text("Invalid input")),
                            small(None, text(DateTimeOffset.UtcNow.ToString(CultureInfo.CurrentUICulture))),
                            button(new[] { type("button"), @class("ml-2 mb-1 close"), attribute("data-dismiss", "toast") }, Elements.span(None, text("🗙")))),
                        div(
                            new[] { @class("toast-body") },
                            FormatErrorMessages(currentViewModel.Errors)
                        )))));

    private static IEnumerable<IAttribute> None
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

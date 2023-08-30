using Microsoft.AspNetCore.Components;
using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;

namespace Radix.Components.Material._3._2._0.AppBar.Top.Action;

public interface Button { }

public abstract class Button<TModel, TCommand> : Component<TModel, TCommand>, Button
    where TModel : ButtonModel
    where TCommand : ButtonCommand<TCommand>
{
    [Parameter] public EventCallback OnClick { get; set; }

    protected override Node View(TModel model, Action<TCommand> dispatch) =>
        button
        (
            [
                @class(new []{"material-icons", "mdc-top-app-bar__action-item", "mdc-icon-button"}),
                attribute("aria-label", new []{model.AriaLabel}),
                on.click(async _ => await OnClick.InvokeAsync())
            ],
            [
                text
                (
                    model.Name
                )
            ]

        );
};





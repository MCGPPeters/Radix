
using Microsoft.AspNetCore.Components;

using Radix.Interaction.Data;
using Radix.Interaction.Web;



namespace Radix.Components.Material._3._2._0.AppBar.Top.Navigation;

public interface Button { }

public abstract class Button<TModel, TCommand> : Component<TModel, TCommand>, Button
    where TModel : ButtonModel
    where TCommand : ButtonCommand<TCommand>
{
    [Parameter] public EventCallback<string> OnClick { get; set; }

    public override Node[] View(TModel model, Func<TCommand, Task> dispatch) =>

    [
        button
        (
            [
                @class(new[] { "material-icons", "mdc-top-app-bar__navigation-icon", "mdc-icon-button" }),
                attribute("aria-label", new[] { model.AriaLabel }),
                on.click(_ => dispatch(TCommand.Create()))
            ],

            [
                text
                (
                    model.Name
                )
            ]

        )
    ];
}



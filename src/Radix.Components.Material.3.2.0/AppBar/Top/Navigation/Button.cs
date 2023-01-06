
using Microsoft.AspNetCore.Components;
using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;


namespace Radix.Components.Material._3._2._0.AppBar.Top.Navigation;

public interface Button { }

public abstract class Button<TModel, TCommand> : Component<TModel, TCommand>, Button
    where TModel : ButtonModel
    where TCommand : ButtonCommand<TCommand>
{
    [Parameter] public EventCallback<string> OnClick { get; set; }

    protected override View<TModel, TCommand> View =>
            static async (model, dispatch) =>
            {
                return await Task.FromResult(button
                (
                    new[]
                    {
                        @class(new[]{"material-icons", "mdc-top-app-bar__navigation-icon", "mdc-icon-button"}),
                        attribute("aria-label", new[] { model.AriaLabel }),
                        on.click((NodeId)3, _ => dispatch(TCommand.Create()))
                    },
                    new[]
                    {
                        text
                        (
                            model.Name
                        )
                    }

                ));
            };

}

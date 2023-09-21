
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Components;
using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web;
using Radix.Interaction.Web.Components;


namespace Radix.Components.Material._3._2._0.AppBar.Top.Navigation;

public interface Button { }

public abstract class Button<TModel, TCommand> : Component<TModel, TCommand>, Button
    where TModel : ButtonModel
    where TCommand : ButtonCommand<TCommand>
{
    [Parameter] public EventCallback<string> OnClick { get; set; }

    public override Node View(TModel model,Func<TCommand, Task> dispatch) =>
            button
                (
                    new[]
                    {
                        @class(new[]{"material-icons", "mdc-top-app-bar__navigation-icon", "mdc-icon-button"}),
                        attribute("aria-label", new[] { model.AriaLabel }),
                        on.click( _ => dispatch(TCommand.Create()))
                    },
                    new[]
                    {
                        text
                        (
                            model.Name
                        )
                    }

                );
            };



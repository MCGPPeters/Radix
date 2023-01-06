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

    protected override View<TModel, TCommand> View =>
            async (model, dispatch) =>
            {
                return await Task.FromResult(button
                        (
                            new[]
                            {
                                @class((NodeId)2, "material-icons", "mdc-top-app-bar__action-item", "mdc-icon-button"),
                                attribute((NodeId)3, "aria-label", model.AriaLabel),
                                on.click((NodeId)4, async _ => await OnClick.InvokeAsync())
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




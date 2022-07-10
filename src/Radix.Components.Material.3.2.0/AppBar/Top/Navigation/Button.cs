﻿
using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;


namespace Radix.Components.Material._3._2._0.AppBar.Top.Navigation;

public interface Button { }

public abstract class Button<TModel, TCommand> : Component<TModel, TCommand>, Button
    where TModel : ButtonModel
    where TCommand : ButtonCommand<TCommand>
{

    protected override View<TModel, TCommand> View =>
            static async (model, dispatch) =>
            {
                return button
                (
                    (NodeId)1,
                    new[]
                    {
                        @class((NodeId)1, "material-icons", "mdc-top-app-bar__navigation-icon", "mdc-icon-button"),
                        attribute((NodeId)2, "aria-label", model.AriaLabel),
                        on.click((NodeId)3, mouseEventArgs => dispatch(TCommand.Create()))
                    },
                    text
                    (
                        (NodeId)4,
                        model.Name
                    )
                );
            };

}

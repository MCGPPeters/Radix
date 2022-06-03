
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
            async (model, dispatch) =>
            {
                return button
                (
                    (NodeId)1,
                    new[]
                    {
                        @class((AttributeId)1, "material-icons", "mdc-top-app-bar__navigation-icon", "mdc-icon-button"),
                        attribute((AttributeId)2, "aria-label", model.AriaLabel),
                        on.click((AttributeId)3, mouseEventArgs => dispatch(TCommand.Create()))
                    },
                    text
                    (
                        (NodeId)2,
                        model.Name
                    )
                );
            };

}

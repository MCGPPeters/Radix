

namespace Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

public class ShoppingCart : Button<ShoppingCartButtonModel, ShoppingCartButtonCommand>
{
    protected override Interaction.Update<ShoppingCartButtonModel, ShoppingCartButtonCommand> Update => (model, command) => Task.FromResult(model);
}

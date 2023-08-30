

namespace Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

public class ShoppingCart : Button<ShoppingCartButtonModel, ShoppingCartButtonCommand>
{
    protected override async ValueTask<ShoppingCartButtonModel> Update(ShoppingCartButtonModel model, ShoppingCartButtonCommand command) => model;
}

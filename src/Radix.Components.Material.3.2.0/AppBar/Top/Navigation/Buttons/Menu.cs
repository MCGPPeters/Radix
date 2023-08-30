

namespace Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons;

public class Menu : Button<MenuButtonModel, MenuButtonCommand>
{
    protected override async ValueTask<MenuButtonModel> Update(MenuButtonModel model, MenuButtonCommand _) => model;
}

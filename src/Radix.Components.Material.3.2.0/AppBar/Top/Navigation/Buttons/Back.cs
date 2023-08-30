
using Radix.Components.Material._3._2._0.AppBar.Top.Action;

namespace Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons;

public class Back : Button<BackButtonModel, BackButtonCommand>
{
    protected override async ValueTask<BackButtonModel> Update(BackButtonModel model, BackButtonCommand _) => model;
}



namespace Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

public class Options(OptionsButtonModel Model) : Button<OptionsButtonModel, OptionsButtonCommand>
{
    public override async ValueTask<OptionsButtonModel> Update(OptionsButtonModel model, OptionsButtonCommand command) => model;
}

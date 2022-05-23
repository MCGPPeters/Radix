

namespace Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

public class Options : Button<OptionsButtonModel, OptionsButtonCommand>
{
    protected override Interaction.Update<OptionsButtonModel, OptionsButtonCommand> Update => (model, command) => Task.FromResult(model);
}

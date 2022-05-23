

namespace Radix.Components.Material._3._2._0.AppBar.Top.Navigation.Buttons;

public class Up : Button<UpButtonModel, UpButtonCommand>
{
    protected override Interaction.Update<UpButtonModel, UpButtonCommand> Update => (model, _) => Task.FromResult(model);
}

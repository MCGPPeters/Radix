namespace Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

public class Search : Button<SearchButtonModel, SearchButtonCommand>
{
    protected override Interaction.Update<SearchButtonModel, SearchButtonCommand> Update => (model, command) => Task.FromResult(model);
}

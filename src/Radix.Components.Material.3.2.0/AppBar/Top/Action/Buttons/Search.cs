using Microsoft.JSInterop;

namespace Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

public class Search : Button<SearchButtonModel, SearchButtonCommand>
{
    public Search()
    {
        // OnClick = async _ => await JSRuntime.InvokeAsync<object>("showSearchBar", Array.Empty<object>());
    }
    protected override Interaction.Update<SearchButtonModel, SearchButtonCommand> Update => async (model, command) => model;
}

namespace Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

public class Search : Button
{
    public Search()
    {
        OnClick = async _ => await JSRuntime.InvokeAsync<object>("showSearchBar", Array.Empty<object>());
    }

    protected override string Name => "search";

    protected override string AriaLabel => "Search";

}

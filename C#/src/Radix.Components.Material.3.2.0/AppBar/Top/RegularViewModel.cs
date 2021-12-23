namespace Radix.Components.Material._3._2._0.AppBar.Top;

public record RegularViewModel : ViewModel
{
    public string? PageTitle { get; set; }
    public Navigation.Button? NavigationButton { get; set; }

    public List<Action.Button> ActionButtons { get; set; } = new List<Action.Button>();
    public string? Id { get; set; }
}

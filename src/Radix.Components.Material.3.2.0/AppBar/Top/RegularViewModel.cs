using Radix.Components.Material._3._2._0.AppBar.Top.Navigation;

namespace Radix.Components.Material._3._2._0.AppBar.Top;

public record RegularModel
{
    public string? PageTitle { get; set; }
    public Button? NavigationButton { get; set; }

    public List<Action.Button> ActionButtons { get; set; } = new List<Action.Button>();
    public string? Id { get; set; }
    public string SearchTerm { get; internal set; }

    internal Task Search(string searchTerm) => throw new NotImplementedException();
}

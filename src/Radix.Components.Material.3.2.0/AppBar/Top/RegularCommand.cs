namespace Radix.Components.Material._3._2._0.AppBar.Top
{
    public interface RegularCommand
    {
    }

    public record SearchCommand(string SearchTerm) : RegularCommand;
}

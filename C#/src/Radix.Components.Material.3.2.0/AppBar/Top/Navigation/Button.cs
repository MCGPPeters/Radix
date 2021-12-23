using Radix.Components.Html;

namespace Radix.Components.Material._3._2._0.AppBar.Top.Navigation;

public abstract class Button : Component
{
    protected abstract string Name { get; }
    protected abstract string AriaLabel { get; }

    protected override Node View() =>
        button
        (
            new[]
            {
                    @class("material-icons mdc-top-app-bar__navigation-icon mdc-icon-button"),
                    attribute("aria-label", AriaLabel)
            },
            text
            (
                Name
            )
        );
}

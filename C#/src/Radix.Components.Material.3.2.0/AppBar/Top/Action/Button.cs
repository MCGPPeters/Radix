using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radix.Components.Html;

namespace Radix.Components.Material._3._2._0.AppBar.Top.Action;

public abstract class Button : Component
{
    protected abstract string Name { get; }
    protected abstract string AriaLabel { get; }

    protected override Node View() =>
        button
        (
            new[]
            {
                    @class("material-icons mdc-top-app-bar__action-item mdc-icon-button"),
                    attribute("aria-label", AriaLabel)
            },
            text
            (
                Name
            )
        );
}

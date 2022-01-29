using Radix.Components;
using Radix.Components.Html;
using Radix.Shop.Catalog.Interface.Logic.Components.Jumbo;

namespace Radix.Shop.Catalog.Interface.Logic.Components.AH;

/// <summary>
///
/// </summary>
public class LogoReference : Component<LogoReferenceViewModel>
{
    protected override Node View(LogoReferenceViewModel currentViewModel) =>
        img
        (
            height("45"),
            width("45"),
            src("/images/ah.svg")
        );
}

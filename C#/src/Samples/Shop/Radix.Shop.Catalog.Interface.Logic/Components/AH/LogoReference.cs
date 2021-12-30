using Radix.Components;
using Radix.Components.Html;
using Radix.Shop.Catalog.Interface.Logic.Components.Jumbo;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;

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

using Radix.Components;
using Radix.Components.Html;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Attributes;

namespace Radix.Shop.Catalog.Interface.Logic.Components.Jumbo;

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
            src("https://www.jumbo.com/INTERSHOP/static/WFS/Jumbo-Grocery-Site/-/-/nl_NL/images/favicon.ico")
        );
}

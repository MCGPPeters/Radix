using Radix.Components;
using Radix.Components.Html;
using static Radix.Components.Html.Elements;
using static Radix.Components.Html.Components;
using static Radix.Components.Html.Attributes;
using Radix.Shop.Catalog.Interface.Logic.Components;

namespace Tsheap.Com.Components;

public class List : Component<ListViewModel>
{
    protected override Node View(ListViewModel currentViewModel)
    {
        return concat
            (
                component<Carousel>
                (
                    Array.Empty<IAttribute>(),
                    Array.Empty<Node>()
                )
            );
    }
}

public record ListViewModel : ViewModel
{
}


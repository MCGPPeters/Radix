using Radix.Components;
using Radix.Components.Html;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

public class List : Component<ListViewModel>
{
    protected override Node View(ListViewModel currentViewModel)
    {
        return concat
            (
                component<Carousel>
                (
                    Array.Empty<Radix.Components.Attribute>(),
                    Array.Empty<Node>()
                )
            );
    }
}

public record ListViewModel : ViewModel
{
}


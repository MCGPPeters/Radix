using Radix.Components;
using Radix.Components.Html;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

public record CarouselViewModel 
(
    string Id, 
    CarouselOptions Options,
    params Node[] Children
) : ViewModel;


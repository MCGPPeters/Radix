using Radix.Components;
using Radix.Components.Html;

namespace Tsheap.Com.Components;

public record CarouselViewModel 
(
    string Id, 
    CarouselOptions Options,
    params Node[] Children
) : ViewModel;


using Radix.Components;
using Radix.Interaction.Data;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

public record CarouselModel 
(
    string Id, 
    CarouselOptions Options,
    params Node[] Children
);


using Radix.Interaction;
using Radix.Interaction.Data;

namespace Radix.Shop.Catalog.Interface.Logic.Components;

public class List : Component<ListModel, ListCommand>
{
    protected override View<ListModel, ListCommand> View =>
        async (model, dispatch) =>
        {
            return concat
            (
                (NodeId)1,
                component<Carousel>
                (
                    (NodeId)2,
                    Array.Empty<Interaction.Data.Attribute<object>>(),
                    Array.Empty<Node>()
                )
            );
        };

    protected override Interaction.Update<ListModel, ListCommand> Update => (model, _) => Task.FromResult(model);

}


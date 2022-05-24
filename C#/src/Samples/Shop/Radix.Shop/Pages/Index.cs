using Microsoft.AspNetCore.Components;
using Radix.Interaction;
using Radix.Interaction.Components;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using Radix.Shop.Catalog.Interface.Logic.Components;

namespace Radix.Shop.Pages
{
    [Route("/")]
    public class Index : Component<IndexModel, object>
    {
        protected override View<IndexModel, object> View =>
            async (model, _) =>
                concat
                (
                    (NodeId)1,
                    component<Search>
                    (
                        (NodeId)2,
                        Array.Empty<ComponentAttribute>(),
                        Array.Empty<Node>()
                    )
                );

        protected override Interaction.Update<IndexModel, object> Update => (model, _) => Task.FromResult(model);

                     
    }
}

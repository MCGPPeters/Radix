using Microsoft.AspNetCore.Components;
using Radix.Interaction;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using static Radix.Interaction.Components.Prelude;
using Node = Radix.Interaction.Data.Node;

namespace Radix.Shop.Wasm.Pages
{
    [Route("/")]
    public class Index : Component<IndexModel, object>
    {
        protected override View<IndexModel, object> View => async (_, __) =>
            text((NodeId)1, "Hello!");

        protected override Interaction.Update<IndexModel, object> Update => (model, _) => Task.FromResult(model);

    }

    public class IndexModel
    {
    }
}

using Radix.Interaction;
using Radix.Interaction.Components;
using Radix.Interaction.Components.Nodes;
using Radix.Interaction.Data;
using Radix.Interaction.Web.Components;
using static Radix.Interaction.Web.Components.Components;
using static Radix.Interaction.Components.Prelude;
using Attribute = Radix.Interaction.Data.Attribute;
using static Radix.Interaction.Components.Prelude;
using static Radix.Interaction.Web.Components.Nodes.Elements;
using static Radix.Interaction.Web.Components.Attributes;
using Microsoft.AspNetCore.Components;

namespace Radix.Interaction.Web.Demo.Pages;

[Route("/")]
public class Index : Component<IndexModel, IndexCommand>
{
    protected override View<IndexModel, IndexCommand> View =>
        (model, dispatch) =>
            Task.FromResult((Node)concat(
            [
                h1
                (
                    [],
                    [
                        text
                        (
                            "Home"
                        )
                    ]
                ),
                text
                (
                    "Welcome to your new app."
                )                
            ]));

    protected override Update<IndexModel, IndexCommand> Update => (model, command) => Task.FromResult(model);
}

public record IndexCommand
{
}

public record IndexModel
{
}

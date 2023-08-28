using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Radix.Interaction.Web.Demo.Pages;

[Route("/")]
public class Index : Component<IndexModel, IndexCommand>
{
    protected override View<IndexModel, IndexCommand> View =>
        (model, dispatch) =>
            Task.FromResult((Node)concat(
            [
                component<PageTitle>
                (
                    [],
                    [
                        text("Home")
                    ]
                ),
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

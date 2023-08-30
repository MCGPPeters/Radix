using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Radix.Interaction.Web.Demo.Pages;

[Route("/")]
public class Index : Component<IndexModel, IndexCommand>
{
    protected override View<IndexModel, IndexCommand> View =>
        (model, dispatch) =>
            concat(
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
            ]);

    protected override Update<IndexModel, IndexCommand> Update => async (model, command) => model;
}

public record IndexCommand
{
}

public record IndexModel
{
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Radix.Interaction.Web.Demo.Pages;

[Route("/")]
public class Index : Component<IndexModel, IndexCommand>
{

    protected override async ValueTask<IndexModel> Update(IndexModel model, IndexCommand command) => model;
    protected override Node View(IndexModel model, Action<IndexCommand> dispatch) =>
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
}

public record IndexCommand
{
}

public record IndexModel
{
}

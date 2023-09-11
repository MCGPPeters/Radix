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
                        "Home"
                    ]
                ),
                h1
                (
                    [],
                    [
                        "Home"
                    ]
                ),
                "Welcome to your new app."
            ]);
}

public record IndexCommand
{
}

public record IndexModel
{
}

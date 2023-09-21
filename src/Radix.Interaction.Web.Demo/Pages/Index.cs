using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Radix.Interaction.Web.Demo.Pages;

[Route("/")]
public class Index : Component
{
    public override Node Render() =>
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
                        text("Home")
                    ]
                ),
                text("Welcome to your new app.")
            ]);
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Radix.Interaction.Web.Demo.Pages;

[Route("/")]
public class Index : Component
{
    public override Node[] Render() =>
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
                    [
                        text("Home")
                    ]
                ),
                text("Welcome to your new app."),
                component<SurveyPrompt>
                (
                    [
                        attribute("Title", ["How is Blazor working for you? "])
                    ],
                    []
                )
            ];
}

public class SurveyPrompt : Component
{
    public override Node[] Render() =>
    [
        div
        (
            [
                @class(["alert alert-secondary mt-4"])
            ],
            [
                span
                (
                    [
                        @class(["oi oi-pencil me-2"]),
                        aria_hidden(["true"])
                    ],
                    []
                ),
                strong
                (
                    [
                        text(Title)
                    ]
                ),
                span
                (
                    [
                        @class(["text-nowrap"])
                    ],
                    [
                        text("Please take our "),
                        a
                        (
                            [
                                href(["https://go.microsoft.com/fwlink/?linkid=2186158"]),
                                @class(["font-weight-bold link-dark"])
                            ],
                            [
                                text("brief survey")
                            ]
                        ),
                        text(" and tell us what you think.")
                    ]
                )
            ]
        )
    ];


    [Parameter]
    public string? Title { get; set; }
}

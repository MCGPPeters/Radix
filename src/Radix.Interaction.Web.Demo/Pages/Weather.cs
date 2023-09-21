using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Radix.Interaction.Web.Demo.Pages;

[Route("/weather")]
[StreamRendering(true)]
public class Weather : Component<WeatherModel>
{        


    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    protected override async Task OnInitializedAsync()
    {
        // Simulate retrieving the data asynchronously.
        await Task.Delay(1000);

        var startDate = DateOnly.FromDateTime(DateTime.Now);
        Model.Forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();
    }

    public override Node Render() =>
        concat(
                [
                    component<PageTitle>
                    (
                        [],
                        [
                            text("Weather")
                        ]
                    ),
                    h1
                    (
                        [],
                        [
                            text("Weather")
                        ]
                    ),
                    p
                    (
                        [],
                        [
                            text("This component demonstrates showing data from the server.")
                        ]
                    ),
                    Model.Forecasts is null
                    ?
                        p
                        (
                            [],
                            [
                                em
                                (
                                    [],
                                    [
                                        text("Loading...")
                                    ]
                                )
                            ]
                        )
                    :
                        table
                        (
                            [
                                @class(["table"])
                            ],
                            [
                                thead
                                (
                                    [],
                                    [
                                        tr
                                        (
                                            [],
                                            [
                                            th
                                            (
                                                [],
                                                [
                                                    text("Date")
                                                ]
                                            ),
                                            th
                                            (
                                                [],
                                                [
                                                    text("Temp. (C)")
                                                ]
                                            ),
                                            th
                                            (
                                                [],
                                                [
                                                    text("Temp. (F)")
                                                ]
                                            ),
                                            th
                                            (
                                                [],
                                                [
                                                    text("Summary")
                                                ]
                                            )
                                            ]
                                        )
                                    ]
                                ),
                                tbody
                                (
                                    []
                                    ,
                                    Model.Forecasts.Select(
                                        forecast =>
                                            tr
                                            (
                                                [],
                                                [
                                                    td
                                                    (
                                                        [],
                                                        [
                                                            text(forecast.Date.ToShortDateString())
                                                        ]
                                                    ),
                                                    td
                                                    (
                                                        [],
                                                        [
                                                            text(forecast.TemperatureC.ToString())
                                                        ]
                                                    ),
                                                    td
                                                    (
                                                        [],
                                                        [
                                                            text(forecast.TemperatureC.ToString())
                                                        ]
                                                    ),
                                                    td
                                                    (
                                                        [],
                                                        [
                                                            text(forecast.Summary ?? "")
                                                        ]
                                                    )
                                                ]
                                            )
                                        ).ToArray()
                                )
                            ]
                        )

                    ]
                );
}

public record WeatherModel
{
    public WeatherForecast[]? Forecasts { get; internal set; }
}

public record WeatherCommand
{

}

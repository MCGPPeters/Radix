﻿using Microsoft.AspNetCore.Components.Web;

namespace Radix.Interaction.Web.Demo.Shared;

public class Counter : Component<CounterModel, CounterCommand>
{
    public override Node[] View(CounterModel model, Func<CounterCommand, Task> dispatch) =>
        [
            div
            (
                [],
                [
                    button
                    (
                        [
                            type(["button"]),
                            @class(["btn", "btn-primary"]),
                            on.click(_ => dispatch(new Increment()))
                        ],
                        [
                            text("+")
                        ]
                    ),
                    div
                    (
                        [],
                        [
                           text(model.Count.ToString())
                        ]
                    ),
                    button
                    (
                        [
                            type(["button"]),
                            @class(["btn", "btn-primary"]),
                            on.click
                            (
                                 _ => dispatch(new Decrement())
                            )
                        ],
                        [
                            text("-")
                        ]
                    )
                ]
            )
        ];

    public override async ValueTask<CounterModel> Update(CounterModel model, CounterCommand command) =>
            command switch
            {
                Increment => model with { Count = model.Count + 1 },
                Decrement => model with { Count = model.Count - 1 },
                _ => throw new NotImplementedException(),
            };
}

public interface CounterCommand
{
}

public record Increment : CounterCommand;
public record Decrement : CounterCommand;

public record CounterModel
{
    public required int Count { get; init; }
};

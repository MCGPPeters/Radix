﻿using Radix.Interaction.Data;

namespace Radix.Interaction;

public delegate Task<Node> View<TModel, out TCommand>(TModel model, Action<TCommand> dispatch);

public delegate Task<TModel> Update<TModel, in TCommand>(TModel model, TCommand command);

public delegate void Render(Node node);

public delegate Task Next<TModel, TCommand>(TModel model, View<TModel, TCommand> interact, Update<TModel, TCommand> update, Render render, Action stateHasChanged);

public static class Prelude<TModel, TCommand>
{
    public static Next<TModel, TCommand> Next { get; } =
            async (model, interact, update, render, stateHasChanged) =>
            {
                // process the command and render the output
                var node = await interact(model, async command =>
                {
                    var newModel = await update(model, command);
                    if(Next is not null)
                        await Next(newModel, interact, update, render, stateHasChanged);
                    stateHasChanged();
                });
                render(node); 
            };
}
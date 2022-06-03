

namespace Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

public class Favorite : Button<FavoriteButtonModel, FavoriteButtonCommand>
{
    protected override Interaction.Update<FavoriteButtonModel, FavoriteButtonCommand> Update => (model, command) => Task.FromResult(model);
}

public class FavoriteButtonCommand : ButtonCommand<FavoriteButtonCommand>
{
    public static FavoriteButtonCommand Create() => new();
}

public class FavoriteButtonModel : ButtonModel
{
    public string Name => "favorite";

    public string AriaLabel => "Favorite";
}

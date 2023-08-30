

namespace Radix.Components.Material._3._2._0.AppBar.Top.Action.Buttons;

public class Favorite : Button<FavoriteButtonModel, FavoriteButtonCommand>
{
    protected override async ValueTask<FavoriteButtonModel> Update(FavoriteButtonModel model, FavoriteButtonCommand command) => model;
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

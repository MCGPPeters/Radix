namespace Radix.Components.Material._3._2._0.AppBar.Top.Action;

public interface ButtonCommand<TCommand> where TCommand : ButtonCommand<TCommand>
{
    public static abstract TCommand Create();
}

namespace Radix.Components.Material._3._2._0.AppBar.Top.Navigation
{
    public interface ButtonCommand<TCommand> where TCommand : ButtonCommand<TCommand>
    {
        public static abstract TCommand Create();
    }
}

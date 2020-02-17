namespace Radix
{
    internal interface Agent<TCommand>
    {
        void Post(CommandDescriptor<TCommand> command);
    }
}
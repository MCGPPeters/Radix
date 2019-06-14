namespace Radix.Tests
{
    internal interface Agent<TCommand>
    {
        void Post(CommandDescriptor<TCommand> command);
    }
}
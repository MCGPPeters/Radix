namespace Radix;

public abstract class CommandDecisionError : CommandProcessingError
{
    internal CommandDecisionError(string message) : base(message)
    {
    }
}

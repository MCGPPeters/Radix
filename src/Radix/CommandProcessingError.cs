namespace Radix;

public class CommandProcessingError : Error
{
    internal CommandProcessingError(string message) : base(message)
    {
    }
}

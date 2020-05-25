namespace Radix
{
    public class CommandDecisionError : CommandProcessingError
    {
        internal CommandDecisionError(string Message) : base(Message)
        {
        }
    }


}

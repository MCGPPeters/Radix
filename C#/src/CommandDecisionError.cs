namespace Radix
{
    public class CommandDecisionError : CommandProcessingError
    {
        public CommandDecisionError(string[] messages)
        {
            Messages = messages;
        }

        public string[] Messages { get; }
    }


}
